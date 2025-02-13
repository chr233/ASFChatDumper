using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using System.Text;

namespace ASFChatDumper.Core;

internal static class Command
{
    /// <summary>
    /// 导出好友聊天记录
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="mixChat"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDumpChat(Bot bot, bool mixChat = false)
    {
        if (!bot.IsConnectedAndLoggedOn)
        {
            return bot.FormatBotResponse(Strings.BotNotConnected);
        }

        var activeSessions = await WebRequest.GetActiveMessageSessions(bot).ConfigureAwait(false);
        if (activeSessions?.Response == null)
        {
            return bot.FormatBotResponse(Langs.NetworkError);
        }

        var session = activeSessions.Response.MessageSessions;
        if (session == null || session.Count == 0)
        {
            return bot.FormatBotResponse("未检测到活跃会话");
        }

        var sb = new StringBuilder();

        Dictionary<ulong, List<DumpChatData>> chatDicts = [];

        foreach (var ss in session)
        {
            var friendId32 = ss.AccountIdFriend;
            var friendId64 = Steam322SteamId(friendId32);
            var friendName = bot.SteamFriends.GetFriendPersonaName(friendId64);

            int count = 0;

            if (!chatDicts.TryGetValue(friendId64, out var chats))
            {
                chats = [];
                chatDicts[friendId64] = chats;
            }

            var recentMessages = await WebRequest.GetRecentMessages(bot, friendId64, 0).ConfigureAwait(false);
            while (true)
            {
                if (recentMessages?.Response?.Messages?.Count > 0)
                {
                    count += recentMessages.Response.Messages.Count;

                    foreach (var message in recentMessages.Response.Messages)
                    {
                        DumpChatData chat;
                        if (message.AccountId == friendId32)
                        {
                            chat = new DumpChatData(friendName, friendId64, bot.BotName, bot.SteamID, message.Message, message.Timestamp);
                        }
                        else
                        {
                            chat = new DumpChatData(bot.BotName, bot.SteamID, friendName, friendId64, message.Message, message.Timestamp);
                        }

                        chats.Add(chat);
                    }

                    if (!recentMessages.Response.MoreAvailable)
                    {
                        break;
                    }

                    var lastTimestamp = recentMessages.Response.Messages.Last().Timestamp;
                    recentMessages = await WebRequest.GetRecentMessages(bot, friendId64, lastTimestamp).ConfigureAwait(false);
                }
                else
                {
                    if (recentMessages?.Response == null)
                    {
                        count = -1;
                    }
                    else
                    {
                        count = 0;
                    }
                    break;
                }
            }

            if (chats.Count == 0)
            {
                chatDicts.Remove(friendId64);
            }

            var msg = count switch
            {
                -1 => "获取失败",
                0 => "无消息",
                _ => $"共获取 {count} 条消息",
            };

            sb.AppendLine(string.Format("{0} {1} {2}", friendId64, friendName, msg));
        }

        if (mixChat)
        {
            var fullName = "output";
            List<DumpChatData> chatList = [];
            foreach (var chats in chatDicts.Values)
            {
                chatList.AddRange(chats);
            }

            await DumpCore.DempChatToCsv(fullName, chatList).ConfigureAwait(false);
        }
        else
        {
            foreach (var (steamId, chats) in chatDicts)
            {
                var name = bot.SteamFriends.GetFriendPersonaName(steamId) ?? "NULL";
                var fullName = $"{steamId}-{name}";
                await DumpCore.DempChatToCsv(fullName, chats).ConfigureAwait(false);
            }
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 导出好友聊天记录 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <param name="mixChat"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDumpChat(string botNames, bool mixChat = false)
    {
        if (string.IsNullOrEmpty(botNames))
        {
            throw new ArgumentNullException(nameof(botNames));
        }

        var bots = Bot.GetBots(botNames);

        if ((bots == null) || (bots.Count == 0))
        {
            return FormatStaticResponse(Strings.BotNotFound, botNames);
        }

        var results = await Utilities.InParallel(bots.Select(bot => ResponseDumpChat(bot, mixChat))).ConfigureAwait(false);
        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
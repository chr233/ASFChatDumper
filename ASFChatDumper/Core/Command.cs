using ArchiSteamFarm.Core;
using ArchiSteamFarm.Localization;
using ArchiSteamFarm.Steam;
using SteamKit2;
using System.Text;

namespace ASFChatDumper.Core;

internal static class Command
{
    /// <summary>
    /// 导出好友聊天记录
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDumpChat(Bot bot)
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
            return bot.FormatBotResponse("无活跃会话");
        }

        var sb = new StringBuilder();
        foreach (var ss in session)
        {
            var steamId64 = Steam322SteamId(ss.AccountIdFriend);
            var steamId = new SteamID(steamId64);
            var name = bot.SteamFriends.GetFriendPersonaName(steamId);

            sb.AppendLine(string.Format("{0} {1}", steamId64, name));

            var recentMessages = await WebRequest.GetRecentMessages(bot, steamId64).ConfigureAwait(false);
            if (recentMessages?.Response?.Messages != null)
            {
                foreach (var message in recentMessages.Response.Messages)
                {
                    sb.AppendLine(string.Format(" - {0} {1} {2}", message.Timestamp, message.AccountId, message.Message));
                }
            }
            else
            {
                sb.AppendLine(" - 无消息");
            }
        }

        return bot.FormatBotResponse(sb.ToString());
    }

    /// <summary>
    /// 导出好友聊天记录 (多个Bot)
    /// </summary>
    /// <param name="botNames"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    internal static async Task<string?> ResponseDumpChat(string botNames)
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

        var results = await Utilities.InParallel(bots.Select(ResponseDumpChat)).ConfigureAwait(false);

        var responses = new List<string?>(results.Where(result => !string.IsNullOrEmpty(result)));

        return responses.Count > 0 ? string.Join(Environment.NewLine, responses) : null;
    }
}
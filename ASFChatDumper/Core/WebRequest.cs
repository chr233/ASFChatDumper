using ArchiSteamFarm.Steam;
using System.Globalization;

namespace ASFChatDumper.Core;

internal static class WebRequest
{
    private static Dictionary<string?, UserDetailData> ProfileSteamId = [];

    /// <summary>
    /// 获取好友列表
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    public static async Task<AbstractResponse<GetFriendListResponse>?> GetFriendsList(Bot bot)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();

        Uri request = new(SteamApiURL, $"/IFriendsListService/GetFriendsList/v1/?access_token={token}");
        Uri referer = new(SteamCommunityURL, "/chat/");

        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AbstractResponse<GetFriendListResponse>>(request, referer: referer).ConfigureAwait(false);
        return response?.Content;
    }

    /// <summary>
    /// 获取活动的会话
    /// </summary>
    /// <param name="bot"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    public static async Task<AbstractResponse<GetActiveMessageSessionsResponse>?> GetActiveMessageSessions(Bot bot)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();

        Uri request = new(SteamApiURL, $"/IFriendMessagesService/GetActiveMessageSessions/v1/?access_token={token}&lastmessage_since=0&only_sessions_with_messages=false");
        Uri referer = new(SteamCommunityURL, "/chat/");

        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AbstractResponse<GetActiveMessageSessionsResponse>>(request, referer: referer).ConfigureAwait(false);
        return response?.Content;
    }

    /// <summary>
    /// 获取最近的聊天记录
    /// </summary>
    /// <param name="bot"></param>
    /// <param name="steamId"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    public static async Task<AbstractResponse<GetRecentMessagesResponse>?> GetRecentMessages(Bot bot, ulong steamId)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();

        Uri request = new(SteamApiURL, $"/IFriendMessagesService/GetRecentMessages/v1/?access_token={token}&steamid1={bot.SteamID}&steamid2={steamId}&most_recent_conversation=0&bbcode_format=true");
        Uri referer = new(SteamCommunityURL, "/chat/");

        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AbstractResponse<GetRecentMessagesResponse>>(request, referer: referer).ConfigureAwait(false);
        return response?.Content;
    }

    private static UserDetailData GetUserDetailData(string? profileLink, string? nickname)
    {
        if (!ProfileSteamId.TryGetValue(profileLink, out var user))
        {
            user = new UserDetailData(nickname, profileLink);
        }

        return user;
    }

    public static async Task<GetChatHistoryResponse?> GetChatHistoryFirstPage(Bot bot)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();

        Uri request = new(SteamHelpURL, "/zh-cn/accountdata/GetFriendMessagesLog");

        var response = await bot.ArchiWebHandler.UrlGetToHtmlDocumentWithSession(request, referer: SteamHelpURL).ConfigureAwait(false);

        if (response?.Content == null)
        {
            return null;
        }

        List<ChatData> chatList = [];

        var eleTrs = response.Content.QuerySelectorAll("table.AccountDataTable>tbody>tr");

        foreach (var eleTr in eleTrs)
        {
            var eleTds = eleTr.QuerySelectorAll("td");

            if (eleTds.Length < 4)
            {
                continue;
            }

            var senderNick = eleTds[0].QuerySelector("a")?.TextContent;
            var senderLink = eleTds[0].QuerySelector("a")?.GetAttribute("href");
            var sender = GetUserDetailData(senderLink, senderNick);

            var receiverNick = eleTds[1].QuerySelector("a")?.TextContent;
            var receiverLink = eleTds[1].QuerySelector("a")?.GetAttribute("href");
            var receiver = GetUserDetailData(receiverLink, receiverNick);

            if (!DateTime.TryParseExact(eleTds[2].TextContent, "yyyy 年 M 月 d 日 tt h:mm CST", null, DateTimeStyles.NoCurrentDateDefault, out var time))
            {
                continue;
            }

            var message = eleTds[3].TextContent;

            var chat = new ChatData(sender, receiver, message, time);
            chatList.Add(chat);
        }

        var eleMore = response.Content.QuerySelector("span.AccountDataLoadMore");
        var moreLink = eleMore?.GetAttribute("data-continuevalue");

        return new GetChatHistoryResponse(chatList, moreLink);
    }
}

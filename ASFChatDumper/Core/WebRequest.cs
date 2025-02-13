using ArchiSteamFarm.Steam;

namespace ASFChatDumper.Core;

internal static class WebRequest
{
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
    /// <param name="startOrdinal"></param>
    /// <returns></returns>
    /// <exception cref="AccessTokenNullException"></exception>
    public static async Task<AbstractResponse<GetRecentMessagesResponse>?> GetRecentMessages(Bot bot, ulong steamId, long startOrdinal)
    {
        var token = bot.AccessToken ?? throw new AccessTokenNullException();

        Uri request = new(SteamApiURL, $"/IFriendMessagesService/GetRecentMessages/v1/?access_token={token}&steamid1={bot.SteamID}&steamid2={steamId}&bbcode_format=true&start_ordinal={startOrdinal}");
        Uri referer = new(SteamCommunityURL, "/chat/");

        var response = await bot.ArchiWebHandler.UrlGetToJsonObjectWithSession<AbstractResponse<GetRecentMessagesResponse>>(request, referer: referer).ConfigureAwait(false);
        return response?.Content;
    }
}

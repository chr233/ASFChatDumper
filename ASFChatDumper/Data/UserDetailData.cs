namespace ASFChatDumper.Data;

internal sealed record UserDetailData
{
    public UserDetailData(string? nickname, string? profileLink)
    {
        Nickname = nickname;
        SteamId = 0;
        ProfileLink = profileLink;
    }

    public string? Nickname { get; set; }
    public ulong SteamId { get; set; }
    public string? ProfileLink { get; set; }
}

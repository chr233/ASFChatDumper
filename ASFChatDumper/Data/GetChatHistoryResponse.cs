namespace ASFChatDumper.Data;

internal sealed record GetChatHistoryResponse
{
    public GetChatHistoryResponse(List<ChatData>? chatList, string? @continue)
    {
        ChatList = chatList;
        Continue = @continue;
    }

    public List<ChatData>? ChatList { get; set; }
    public string? Continue { get; set; }
}

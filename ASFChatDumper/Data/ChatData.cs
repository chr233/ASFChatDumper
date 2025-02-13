namespace ASFChatDumper.Data;
internal sealed record ChatData
{
    public ChatData(UserDetailData? sender, UserDetailData? reciver, string? message, DateTime time)
    {
        Sender = sender;
        Reciver = reciver;
        Message = message;
        Time = time;
    }

    public UserDetailData? Sender { get; set; }
    public UserDetailData? Reciver { get; set; }
    public string? Message { get; set; }
    public DateTime Time { get; set; }
}


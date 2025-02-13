namespace ASFChatDumper.Data;

internal sealed record DumpChatData
{
    public DumpChatData(string? senderName, ulong senderSteamId, string? receiverName, ulong receiverSteamId, string? message, long time)
    {
        SenderName = senderName;
        SenderSteamId = senderSteamId;
        ReceiverName = receiverName;
        ReceiverSteamId = receiverSteamId;
        Message = message;
        Time = DateTimeOffset.FromUnixTimeSeconds(time).UtcDateTime;
        Timestamp = time;
    }

    public string? SenderName { get; init; }
    public ulong SenderSteamId { get; init; }

    public string? ReceiverName { get; init; }
    public ulong ReceiverSteamId { get; init; }

    public string? Message { get; init; }
    public DateTime Time { get; init; }
    public long Timestamp { get; init; }
}

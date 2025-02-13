using System.Text.Json.Serialization;

namespace ASFChatDumper.Data;

internal sealed record GetRecentMessagesResponse
{
    [JsonPropertyName("messages")]
    public List<MessageData>? Messages { get; set; }

    [JsonPropertyName("more_available")]
    public bool MoreAvailable { get; set; }

    public sealed record MessageData
    {
        [JsonPropertyName("accountid")]
        public ulong AccountId { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}

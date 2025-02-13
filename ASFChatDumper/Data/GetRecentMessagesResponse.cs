using System.Text.Json.Serialization;

namespace ASFChatDumper.Data;

internal sealed record GetRecentMessagesResponse
{
    [JsonPropertyName("messages")]
    public List<MessageData>? Messages { get; set; }

    public sealed record MessageData
    {
        [JsonPropertyName("accountid")]
        public long AccountId { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }
}

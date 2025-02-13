using System.Text.Json.Serialization;

namespace ASFChatDumper.Data;


internal sealed record GetActiveMessageSessionsResponse
{
    [JsonPropertyName("message_sessions")]
    public List<MessageSessionData>? MessageSessions { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }

    public sealed record MessageSessionData
    {
        [JsonPropertyName("accountid_friend")]
        public ulong AccountIdFriend { get; set; }
        [JsonPropertyName("last_message")]
        public ulong LastMessage { get; set; }
        [JsonPropertyName("last_view")]
        public ulong LastView { get; set; }
        [JsonPropertyName("unread_message_count")]
        public int UnReadMessageCount { get; set; }
    }
}

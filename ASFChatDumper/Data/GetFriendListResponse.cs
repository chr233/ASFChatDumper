using SteamKit2;
using System.Text.Json.Serialization;

namespace ASFChatDumper.Data;

internal sealed record GetFriendListResponse
{
    [JsonPropertyName("friendslist")]
    public FriendsListData? FriendsList { get; set; }

    public sealed record FriendsListData
    {
        [JsonPropertyName("bincremental")]
        public bool Bincremental { get; set; }

        [JsonPropertyName("friends")]
        public List<FriendData>? Friends { get; set; }
    }

    public sealed record FriendData
    {
        [JsonPropertyName("ulfriendid")]
        public string? UlFriendId { get; set; }

        [JsonPropertyName("efriendrelationship")]
        public EFriendRelationship FriendRelationShip { get; set; }
    }
}

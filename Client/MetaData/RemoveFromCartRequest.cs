using System.Text.Json.Serialization;

namespace Client.MetaData
{
    public class RemoveFromCartRequest
    {
        [JsonPropertyName("userId")]
        public string? UserId { get; set; }
        [JsonPropertyName("productId")]
        public int? ProductId { get; set; }
    }
}

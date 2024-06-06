#nullable disable
using System.Text.Json.Serialization;

namespace Client.MetaData
{
    public class AddToCartRequest
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}

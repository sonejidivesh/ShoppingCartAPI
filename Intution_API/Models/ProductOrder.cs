using System.Text.Json.Serialization;

namespace Intution_API.Models
{
    public class ProductOrder
    {
        public int? ProductId { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        public int? OrderId { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }

    }
}

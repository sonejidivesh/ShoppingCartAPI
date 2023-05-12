using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Intution_API.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        public bool IsActive { get; set; } = true;

        public double Price { get; set; }
        public virtual  ICollection<ProductOrder?> ProductOrder { get; set; }
    }
}

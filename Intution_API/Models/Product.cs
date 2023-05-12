using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Intution_API.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        
        public string Description { get; set; } = string.Empty;

        public int Qty { get; set; }

        public bool IsActive { get; set; } = true;

        public double Price { get; set; }
        public virtual ICollection<ProductOrder?>? ProductOrder { get; set; }


    }
}

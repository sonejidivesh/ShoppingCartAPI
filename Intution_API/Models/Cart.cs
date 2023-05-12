namespace Intution_API.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }

        public int CustomerId { get; set; }
    }
}

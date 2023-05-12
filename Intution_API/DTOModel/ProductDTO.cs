namespace Intution_API.DTOModel
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string ProductName { get; set; }


        public string Description { get; set; } = string.Empty;

        public int Qty { get; set; }

        public bool IsActive { get; set; } = true;

        public double Price { get; set; }


    }
}

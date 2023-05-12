using System.ComponentModel.DataAnnotations;

namespace Intution_API.DTOModel
{
    public class OrderDTO
    {
        public int Id { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }


        [Required]
        public List<ProductDTO> Product { get; set; }

        [Required]
        public double Price { get; set; }


    }
}

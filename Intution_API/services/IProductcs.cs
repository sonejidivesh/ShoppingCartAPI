using Intution_API.DTOModel;
using Intution_API.Models;

namespace Intution_API.services
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> UpdateQty(Product product ,ProductDTO productDetails);
    }
}

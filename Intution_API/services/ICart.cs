using Intution_API.Models;

namespace Intution_API.services
{
    public interface ICart : IRepository<Cart> 
    {
        Task<List<Cart>> GetAll(int customerId);

        
        bool DeleteCartItem(int id);

        Task<bool> DeleteCart(int customerId);
    }
}

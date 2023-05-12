using Intution_API.Models;

namespace Intution_API.services
{
    public interface IOrders:IRepository<Order>
    {
        Task<Order?> UpdateAddress(Order order);

    }
}

using Intution_API.Models;

namespace Intution_API.services
{
    public interface IOrders:IRepository<Order>
    {
        Task<Order?> UpdateAddress(Order order);


        Task<List<Order>> GetAllOrderBasedonCutomerIdAsync(int customerId, int pageSize, int pageIndex);

    }
}

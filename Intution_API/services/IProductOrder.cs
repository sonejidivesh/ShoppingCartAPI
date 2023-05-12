using Intution_API.Models;

namespace Intution_API.services
{
    public interface IProductOrder:IRepository<ProductOrder>
    {
        //Edit order item
       bool DeleteOrderItem(ProductOrder productOrder);
   


    }
}

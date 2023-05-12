using Intution_API.Data;
using Intution_API.Models;

namespace Intution_API.services.Repos
{
    public class ProductOrderRepository : Repository<ProductOrder>, IProductOrder
    {
        public ProductOrderRepository(APIDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public override async Task<ProductOrder> Add(ProductOrder entity)
        {
            try
            {
                await _context.ProductOrders.AddAsync(entity);

                return entity;
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex.Message);
                return entity;

            }


        }

        public bool DeleteOrderItem(ProductOrder productOrder)
        {
            try
            {
                 _context.ProductOrders.Remove(productOrder);
                return true;

            }
            catch (Exception ex) {


                _logger.LogWarning(ex.Message);
                return false;
            
            }
        }
    }
}

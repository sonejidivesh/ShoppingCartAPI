using Intution_API.Data;
using Intution_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Intution_API.services.Repos
{
    public class OrderRepository : Repository<Order>, IOrders
    {
        public OrderRepository(APIDbContext context, ILogger logger) : base(context, logger)
        {
        }


        public override async Task<IEnumerable<Order>> GetAllAsync(int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                return await _context.Orders.Include(x => x.ProductOrder).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex.Message);
                return new List<Order>();

            }

        }

        public async Task<List<Order>> GetAllOrderBasedonCutomerIdAsync(int customerId, int pageSize =10, int pageIndex =1 )
        {
            try
            {
                return await _context.Orders.Include(x => x.ProductOrder).Where(x=>x.CustomerId == customerId).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex.Message);
                return new List<Order>();

            }
        }

        public override async Task<Order?> GetValueAsync(int id)
        {
            try
            {
                return await _context.Orders.Include(x => x.ProductOrder).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex.Message);

                return null;
            }
        }

        public async Task<Order?> UpdateAddress(Order order)
        {
            try
            {
                Order? orderDetails = await _context.Orders.FirstOrDefaultAsync(x => x.Id == order.Id);
                if (orderDetails is not null)
                {
                    orderDetails.DeliveryAddress = order.DeliveryAddress;
                    return orderDetails;
                }

                return null;

            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex.Message);

                return null;
            }

        }



        public override async Task<bool> Delete(Order entity)
        {
            try
            {

                Order? order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == entity.Id && x.IsActive == true);
                if (order is not null)
                {
                    order.IsActive = false;
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

       
    }
}

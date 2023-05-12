using Intution_API.Data;
using Intution_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Intution_API.services.Repos
{
    public class CartRepository : Repository<Cart>, ICart
    {
        public CartRepository(APIDbContext context, ILogger logger) : base(context, logger)
        {
        }


       
        public bool DeleteCartItem(int id)
        {
            try
            {
                var cart = _context.Carts.FirstOrDefault(x => x.Id == id);
                if (cart is not null)
                {
                    _context.Carts.Remove(cart);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<List<Cart>> GetAll(int customerId)
        {
            try
            {
                return await _context.Carts.Include(x=>x.Product).Where(x=>x.CustomerId == customerId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);  
                return new List<Cart>();    
            }
        }

        public async Task<bool> DeleteCart(int customerId)
        {
            try
            {
                var cart = await _context.Carts.Where(x=>x.CustomerId == customerId).ToListAsync();


                if (cart.Count != 0 )
                {
                    _context.Carts.RemoveRange(cart);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        
    }
}

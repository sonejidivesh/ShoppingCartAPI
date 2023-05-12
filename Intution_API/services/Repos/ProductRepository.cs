using Intution_API.Data;
using Intution_API.DTOModel;
using Intution_API.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Intution_API.services.Repos
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(APIDbContext context, ILogger logger) : base(context, logger)
        {

        }

        public override async Task<IEnumerable<Product>> GetAllAsync(int pageSize = 10, int pageIndex = 1)
        {
            try
            {
                return await _context.Products.Where(x => x.IsActive && x.Qty > 0  ).Include(x=>x.ProductOrder).Skip(pageSize*(pageIndex-1)).Take(pageSize).ToListAsync();
            }
            catch (Exception ex){

                _logger.LogWarning(ex.Message);
                return new List<Product> ();

            }
           
        }


        public override async Task<Product?> GetValueAsync(int id)
        {
            try
            {
                Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
                if (product is not null)
                {
                    return product;
                }

                return null;

            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return null;
            }
        }

        public override  async Task<bool> Update(Product entity)
        {
            try
            {
                Product?  product = await _context.Products.FirstOrDefaultAsync(x=>x.Id == entity.Id && x.IsActive ==  true);
                if(product is not null)
                {
                    product.ProductName = entity.ProductName;
                    product.Description = entity.Description;
                    product.Qty = entity.Qty;
                    return true;
                }

                return false;

            }catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

        public async override Task<bool> Delete(Product entity)
        {
            try {

                Product? product = await _context.Products.FirstOrDefaultAsync(x => x.Id == entity.Id && x.IsActive == true);
                if (product is not null)
                {
                    product.IsActive = false;
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateQty(Product p , ProductDTO productDetail)
        {
           using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (p is not null)
                    {
                        if (p.Qty < productDetail.Qty)
                        {
                             transaction.Rollback();
                            _logger.LogWarning("Inssufficient product");
                             return false;
                            
                        }

                        p.Qty -= productDetail.Qty;
                        

                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }

                    transaction.Rollback();
                    _logger.LogWarning("product not found");
                    return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogWarning(ex.Message);
                    return false;
                }

            }
        }
    }
}

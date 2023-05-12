using Intution_API.services;
using Intution_API.services.Repos;
using System.Security.AccessControl;

namespace Intution_API.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private  readonly APIDbContext _context;
        //private readonly ILogger _logger;

        public IProductRepository Products { get; private set; }

        public IOrders Orders { get; private set; }

        public IProductOrder ProductOrder { get; private set; }

        public ICart Carts { get; private set; }


        public UnitOfWork(APIDbContext context,ILoggerFactory loggerFactory)
        {
            _context = context;
           var _logger = loggerFactory.CreateLogger("Logs");
            Products = new ProductRepository(_context, _logger);
            Orders  =  new OrderRepository(_context, _logger);
            ProductOrder =   new ProductOrderRepository(_context, _logger);
            Carts =  new CartRepository(_context, _logger);
        }
        public async Task CompleteAsync()
        {
           await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

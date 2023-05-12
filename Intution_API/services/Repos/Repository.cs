using Intution_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Intution_API.services.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected APIDbContext _context;

        internal DbSet<T> _set;

        protected readonly ILogger _logger;


        public Repository(APIDbContext context,ILogger logger)
        {
            _context = context;
            _logger = logger;
            this._set = _context.Set<T>();
        }
        public virtual async Task<T> Add(T entity)
        {
             await _set.AddAsync(entity);
            return entity;
        }

        public virtual async Task<bool> Delete(T entity)
        {
            _set.Update(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(int pageSize =10 , int pageIndex =  1)
        {
            return await _set.Skip(pageIndex*pageSize).Take(pageSize).ToListAsync();
        }

        public virtual async Task<T?> GetValueAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public virtual async Task<bool> Update(T entity)
        {
            _set.Update(entity);    
            return true; 
        }
    }
}

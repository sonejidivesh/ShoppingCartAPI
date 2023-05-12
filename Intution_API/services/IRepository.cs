namespace Intution_API.services
{
    //genric interface 
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(int pageSize =  10 , int pageIndex = 1);

        Task<T?> GetValueAsync(int id);

        Task<T> Add(T entity);

        Task<bool> Delete(T entity);


        Task<bool> Update(T entity);
    }
}

using System.Linq.Expressions;

namespace WebApplication1.UFW
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id, string? includeProperties = null);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}

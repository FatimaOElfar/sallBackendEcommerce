using WebApplication1.Model;

namespace WebApplication1.UFW
{
    public interface IUnitOfWork
    {
        IGenericRepository<Product> Products { get; }
        IGenericRepository<Order> Orders { get; }
        Task<int> CompleteAsync();
    }
}

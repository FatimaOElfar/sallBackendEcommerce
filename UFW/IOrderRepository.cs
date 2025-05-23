using WebApplication1.Model;

namespace WebApplication1.UFW
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        void Update(Order order);
        void Remove(Order order);

    }
}

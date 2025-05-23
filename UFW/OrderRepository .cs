using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.UFW
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetAllWithUserAsync()
        {
            return await _context.Orders.Include(o => o.User).ToListAsync();
        }

        public async Task<Order?> GetByIdWithUserAsync(int id)
        {
            return await _context.Orders.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}

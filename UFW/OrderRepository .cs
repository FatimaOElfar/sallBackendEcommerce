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
        public async Task AddOrderDetailsAsync(IEnumerable<OrderDetail> details)
        {
            await _context.OrderDetails.AddRangeAsync(details);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }

        public void RemoveOrderDetails(IEnumerable<OrderDetail> details)
        {
            _context.OrderDetails.RemoveRange(details);
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

    }
}

using WebApplication1.Model;

namespace WebApplication1.UFW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<Product> Products { get; private set; }
        public IGenericRepository<Order> Orders { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Products = new GenericRepository<Product>(_context);
            Orders = new GenericRepository<Order>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }


}

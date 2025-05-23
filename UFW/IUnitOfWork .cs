namespace WebApplication1.UFW
{
    public interface IUnitOfWork
    {
        IOrderRepository Orders { get; }
        Task<int> CompleteAsync();

    }
}

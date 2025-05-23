using WebApplication1.DTO.product;

namespace WebApplication1.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public UserDto User { get; set; }
        public List<OrderDetailDto>? OrderDetails { get; set; }

    }
}

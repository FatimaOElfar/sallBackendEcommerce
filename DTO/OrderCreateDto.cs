using WebApplication1.DTO.product;

namespace WebApplication1.DTO
{
    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetailCreateDto> OrderDetails { get; set; } = new List<OrderDetailCreateDto>();


    }
}

namespace WebApplication1.DTO.product
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductDto? Product { get; set; }  
    }

}

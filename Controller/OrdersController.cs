using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Model;
using WebApplication1.UFW;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO.product;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context; 

        public OrdersController(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _context.Orders.Include(o => o.User).ToListAsync();

            var ordersDto = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                User = order.User == null ? null : new UserDto
                {
                    Id = order.User.Id,
                    Username = order.User.Username,
                    Email = order.User.Email,
                    Role = order.User.Role
                }
            }).ToList();

            return Ok(ordersDto);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _context.Orders.Include(o => o.User)
                                    .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                User = order.User == null ? null : new UserDto
                {
                    Id = order.User.Id,
                    Username = order.User.Username,
                    Email = order.User.Email,
                    Role = order.User.Role
                }
            };

            return Ok(orderDto);
        }

        // POST: api/Orders
        [HttpPost]

        public async Task<ActionResult<OrderDto>> PlaceOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            var order = new Order
            {
                UserId = orderCreateDto.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = orderCreateDto.TotalAmount,
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var detailDto in orderCreateDto.OrderDetails)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = detailDto.ProductId,
                    Quantity = detailDto.Quantity,
                    UnitPrice = detailDto.UnitPrice
                };
                order.OrderDetails.Add(orderDetail);
            }

            await _unitOfWork.Orders.AddAsync(order);

            await _unitOfWork.CompleteAsync();

            var savedOrder = await _unitOfWork.Orders.GetByIdAsync(order.Id);

            if (savedOrder == null)
                return NotFound();

            var orderDto = new OrderDto
            {
                Id = savedOrder.Id,
                OrderDate = savedOrder.OrderDate,
                TotalAmount = savedOrder.TotalAmount,
                User = savedOrder.User == null ? null : new UserDto
                {
                    Id = savedOrder.User.Id,
                    Username = savedOrder.User.Username,
                    Email = savedOrder.User.Email,
                    Role = savedOrder.User.Role
                },
                OrderDetails = savedOrder.OrderDetails.Select(od => new OrderDetailDto
                {
                    Id = od.Id,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    Product = od.Product == null ? null : new ProductDto
                    {
                        Id = od.Product.Id,
                        Name = od.Product.Name,
                        Price = od.Product.Price
                    }
                }).ToList()
            };

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, orderDto);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, OrderCreateDto orderUpdateDto)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            order.UserId = orderUpdateDto.UserId;
            order.TotalAmount = orderUpdateDto.TotalAmount;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }
        [HttpPost("AddOrderWithDetails")]
        public async Task<IActionResult> AddOrderWithDetailsAsync([FromBody] OrderCreateDto orderCreateDto)
        {
            var order = new Order
            {
                UserId = orderCreateDto.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = orderCreateDto.TotalAmount,
                OrderDetails = orderCreateDto.OrderDetails.Select(d => new OrderDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    UnitPrice = d.UnitPrice
                }).ToList()
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            var savedOrder = await _unitOfWork.Orders.GetByIdAsync(order.Id, includeProperties: "OrderDetails.Product");

            var orderDto = new OrderDto
            {
                Id = savedOrder.Id,
                OrderDate = savedOrder.OrderDate,
                TotalAmount = savedOrder.TotalAmount,
                User = new UserDto
                {
                },
                OrderDetails = savedOrder.OrderDetails.Select(od => new OrderDetailDto
                {
                    Id = od.Id,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice,
                    Product = new ProductDto
                    {
                        Id = od.Product.Id,
                        Name = od.Product.Name,
                        Description = od.Product.Description,
                        Price = od.Product.Price,
                        ImageUrl = od.Product.ImageUrl
                    }
                }).ToList()
            };

            return Ok(orderDto);
        }

    }
}

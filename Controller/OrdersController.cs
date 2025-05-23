using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Model;
using WebApplication1.UFW;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();

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
            var order = await _unitOfWork.Orders.GetByIdAsync(id);

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
        public async Task<ActionResult<OrderDto>> PlaceOrder(OrderCreateDto orderCreateDto)
        {
            var order = new Order
            {
                UserId = orderCreateDto.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = orderCreateDto.TotalAmount
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            // Load User info after saving
            var savedOrder = await _unitOfWork.Orders.GetByIdAsync(order.Id);

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
                }
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
    }
}

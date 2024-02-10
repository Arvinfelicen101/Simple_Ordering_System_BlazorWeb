using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Server.Data;
using OrderingSystem.Shared;

namespace OrderingSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDataContext _context;

        public OrderController(OrderDataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(Order order)
        {


            await _context.Orders.AddAsync(order)
         ;
            await _context.SaveChangesAsync();

            return new ObjectResult(order)
            {
                StatusCode = 201,
                Value = order,
            };
        }
    }
}

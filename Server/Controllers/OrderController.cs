using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.Server.Data;
using OrderingSystem.Shared;
using OrderingSystem.Shared.ViewModels;

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
        public async Task<ActionResult<OrderViewModel>> CreateOrder(OrderViewModel orderViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCustomer = await _context.Customers.FindAsync(orderViewModel.CustomerCustCode);
            if (existingCustomer == null)
            {
                return BadRequest("Invalid customer code.");
            }

            var existingProduct = await _context.Products.FindAsync(orderViewModel.ProductProdCode);
            if (existingProduct == null)
            {
                return BadRequest("Invalid product code.");
            }

            var order = new Order
            {
                OrderNo = orderViewModel.OrderNo,
                OrderDate = orderViewModel.OrderDate,
                Customer = existingCustomer,
                Product = existingProduct,
                Qty = orderViewModel.Qty,
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

        
            var createdOrderViewModel = new OrderViewModel
            {
                OrderId = order.OrderId,
                OrderNo = order.OrderNo,
                OrderDate = order.OrderDate,
                CustomerCustCode = order.Customer.CustCode,
                ProductProdCode = order.Product.ProdCode,
                Qty = order.Qty,
                Customer = CustomerViewModel.FromCustomer(order.Customer),
                Product = ProductViewModel.FromProduct(order.Product)
            };

        
            return new ObjectResult(createdOrderViewModel)
            {
                StatusCode = 201,
                Value = createdOrderViewModel
            };
        }

    
    }
}

    

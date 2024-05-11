using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using OrderingSystem.Server.Data;
using OrderingSystem.Shared;
using OrderingSystem.Shared.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static OrderingSystem.Server.Data.OrderDataContext;

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

        /* var query = _context.Orders.
                     Include(o => o.Customer)
                     .Include(o => o.Product)
                     .AsQueryable(); 

        [HttpGet]
        public async Task<ActionResult<PagedList<OrderViewModel>>> GetOrders(
          int page = 1,
          int pageSize = 10,
          string searchTerm = "")
        {
            try
            {
                Console.WriteLine($"Requesting customers - Page: {page}, PageSize: {pageSize}, SearchTerm: {searchTerm}");

                var query = _context.Orders.
                     Include(o => o.Customer)
                     .Include(o => o.Product)
                     .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(c =>
                        c.CustomerCustCode.Contains(searchTerm) ||
                        c.OrderNo.ToString().Contains(searchTerm) ||
                        c.OrderDate.ToString().Contains(searchTerm) 
                    );
                }

                var totalItems = await query.CountAsync();

                var orders = await query
                    .OrderBy(c => c.OrderNo)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var orderViewModels = orders.Select(o => new OrderViewModel
                {
                  
                    OrderNo = o.OrderNo,
                    CustomerCustCode = o.Customer.CustCode,
                    OrderDate = o.OrderDate,
                   
                }).ToList();

                var pagedList = new PagedList<OrderViewModel>(orderViewModels, totalItems, page, pageSize);

                return Ok(pagedList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: Error");
                return StatusCode(500, "Internal Server Error");
            }
        }
        */

        [HttpGet]
        public async Task<ActionResult<List<OrderViewModel>>> GetOrders()
        {
            var orders = await _context.Orders
           .Include(m => m.Customer)
           .Include(m => m.Product)
           .OrderByDescending(o => o.RowNo)
           .Select(order => new OrderViewModel
           {
               RowNo = order.RowNo,
               OrderNo = order.OrderNo,
               CustomerCustCode = order.CustomerCustCode,
               ProductProdCode = order.ProductProdCode,
               OrderDate = order.OrderDate,
               Qty = order.Qty,
               Price = order.Price,
               Customer = new CustomerViewModel
               {
                   CustCode = order.Customer.CustCode,
                   FullName = order.Customer.FullName,
                   BillAddress = order.Customer.BillAddress,
                   ShipAddress = order.Customer.ShipAddress,
                   // Map other customer properties as needed
               },
               Product = new ProductViewModel
               {
                   ProdCode = order.Product.ProdCode,
                   Name = order.Product.Name,
               }
               // Map other properties as needed
           })
                    .ToListAsync();

            return Ok(orders);
        }



        [HttpPost]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> CreateOrders(IEnumerable<OrderViewModel> orderViewModels, int pOrderNo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Get the latest OrderNo
                int latestOrderNo = await _context.Orders.MaxAsync(o => (int?)o.OrderNo) ?? 0;
                latestOrderNo++;

                foreach (var orderViewModel in orderViewModels)
                {
                    var existingCustomer = await _context.Customers.FindAsync(orderViewModel.CustomerCustCode);
                    if (existingCustomer == null)
                    {
                        return BadRequest($"Invalid customer code for order with OrderNo {orderViewModel.OrderNo}.");
                    }

                    var existingProduct = await _context.Products.FindAsync(orderViewModel.ProductProdCode);
                    if (existingProduct == null)
                    {
                        return BadRequest($"Invalid product code for order with OrderNo {orderViewModel.OrderNo}.");
                    }

                    // Increment the latest OrderNo
                    orderViewModel.OrderNo = latestOrderNo;

                    var order = new Order
                    {
                        RowNo = orderViewModel.RowNo,
                        OrderNo = orderViewModel.OrderNo,
                        OrderDate = orderViewModel.OrderDate,
                        Customer = existingCustomer,
                        Product = existingProduct,
                        Qty = orderViewModel.Qty,
                        Price = orderViewModel.Price
                    };

                    Console.WriteLine($"Creating Order: {order}");

                    await _context.Orders.AddAsync(order);
                }

                await _context.SaveChangesAsync();

                return new ObjectResult(orderViewModels)
                {
                    StatusCode = 201,
                    Value = orderViewModels
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating orders: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }




        [HttpPost("add-order")]
        public async Task<ActionResult<OrderViewModel>> CreateOrder(OrderViewModel orderViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve existing customer
            var existingCustomer = await _context.Customers.FindAsync(orderViewModel.CustomerCustCode);
            if (existingCustomer == null)
            {
                return BadRequest($"Invalid customer code for order with OrderNo {orderViewModel.OrderNo}.");
            }

            // Retrieve existing product
            var existingProduct = await _context.Products.FindAsync(orderViewModel.ProductProdCode);
            if (existingProduct == null)
            {
                return BadRequest($"Invalid product code for order with OrderNo {orderViewModel.OrderNo}.");
            }

            // Populate product and customer properties in orderViewModel


            // Create order entity
             var order = new Order
                {
                    RowNo = orderViewModel.RowNo,
                    OrderNo = orderViewModel.OrderNo,
                    OrderDate = orderViewModel.OrderDate,
                    Customer = existingCustomer,
                    Product = existingProduct,
                    Qty = orderViewModel.Qty,
                    Price = orderViewModel.Price
                };


            Console.WriteLine($"Creating Order: {order}");

            // Add order to database
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Return the created orderViewModel
            return new ObjectResult(orderViewModel)
            {
                StatusCode = 201,
                Value = orderViewModel
            };
        }


        [HttpGet("nextRowNo/{orderNo}")]
        public async Task<ActionResult<int>> GetNextRowNo(int orderNo)
        {
            try
            {
                var maxRowNoOrder = await _context.Orders
                    .Where(o => o.OrderNo == orderNo)
                    .OrderByDescending(o => o.RowNo)
                    .FirstOrDefaultAsync();

                int nextRowNo = maxRowNoOrder != null ? maxRowNoOrder.RowNo + 1 : 1;

                return Ok(nextRowNo); // Return the next row number as a JSON response
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while getting next RowNo: {ex.Message}");
                return StatusCode(500, "Internal server error"); // Return a 500 error if an exception occurs
            }
        }

        [HttpGet("latest-order-number")]
        public async Task<ActionResult<int>> GetLatestOrderNumber()
        {
            try
            {
                int latestOrderNo = await _context.Orders.MaxAsync(o => (int?)o.OrderNo) ?? 0;
               
                return Ok(latestOrderNo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting latest order number: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("searchOrders")]
        public async Task<ActionResult<OrderViewModel>> SearchOrder(string searchTerm)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Product)
                    .FirstOrDefaultAsync(o =>
                        o.OrderNo.ToString() == searchTerm);

                if (order == null)
                {
                    return NotFound();
                }

                var orderViewModel = new OrderViewModel
                {
                    OrderNo = order.OrderNo,
                    CustomerCustCode = order.Customer.CustCode,
                    OrderDate = order.OrderDate,
                    // Map other properties as needed
                };

                return Ok(orderViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("searchOrder")]
        public async Task<ActionResult<List<OrderViewModel>>> GetOrdersByOrderNo(string orderNo)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Product)
                    .Where(o => o.OrderNo.ToString() == orderNo)
                    .OrderBy(o => o.RowNo)
                    .Select(order => new OrderViewModel
                    {
                       RowNo = order.RowNo,
                        OrderNo = order.OrderNo,
                        CustomerCustCode = order.CustomerCustCode,
                        ProductProdCode = order.ProductProdCode,
                        OrderDate = order.OrderDate,
                        Qty = order.Qty,
                        Price = order.Price,
                        Customer = new CustomerViewModel
                        {
                            CustCode = order.Customer.CustCode,
                            FullName = order.Customer.FullName,
                            BillAddress = order.Customer.BillAddress,
                            ShipAddress = order.Customer.ShipAddress,
                            // Map other customer properties as needed
                        },
                        Product = new ProductViewModel
                        {
                            ProdCode = order.Product.ProdCode,
                            Name = order.Product.Name,
                        }
                        // Map other properties as needed
                    })
                    .ToListAsync();

                if (orders == null || orders.Count == 0)
                {
                    return NotFound();
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("orderbyNo")]
        public async Task<ActionResult<List<OrderViewModel>>> GetOrdersByOrderNum(int orderNo)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Product)
                    .Where(o => o.OrderNo == orderNo)
                    .OrderBy(o => o.RowNo)
                    .Select(order => new OrderViewModel
                    {
                        RowNo = order.RowNo,
                        OrderNo = order.OrderNo,
                        CustomerCustCode = order.CustomerCustCode,
                        ProductProdCode = order.ProductProdCode,
                        OrderDate = order.OrderDate,
                        Qty = order.Qty,
                        Price = order.Price,
                        Customer = new CustomerViewModel
                        {
                            CustCode = order.Customer.CustCode,
                            FullName = order.Customer.FullName,
                            BillAddress = order.Customer.BillAddress,
                            ShipAddress = order.Customer.ShipAddress,
                            // Map other customer properties as needed
                        },
                        Product = new ProductViewModel
                        {
                            ProdCode = order.Product.ProdCode,
                            Name = order.Product.Name,
                        }
                        // Map other properties as needed
                    })
                    .ToListAsync();

                if (orders == null || orders.Count == 0)
                {
                    return NotFound();
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{rowNo}/{productProdCode}/{orderNo}")]
        public async Task<ActionResult<OrderViewModel>> GetOrderByRowCodeOrderNo(int rowNo, string productProdCode, int orderNo)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(o => o.RowNo == rowNo &&
                        o.ProductProdCode == productProdCode &&
                        o.OrderNo == orderNo);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            var orderViewModel = new OrderViewModel
            {
                RowNo = order.RowNo,
                OrderId = order.OrderId,
                OrderNo = order.OrderNo,
                OrderDate = order.OrderDate,
                CustomerCustCode = order.CustomerCustCode,
                ProductProdCode = order.ProductProdCode,
                Qty = order.Qty,
                Price = order.Price,
                Customer = new CustomerViewModel
                {
                    CustCode = order.Customer.CustCode,
                    FullName = order.Customer.FullName,
                    BillAddress = order.Customer.BillAddress,
                    ShipAddress = order.Customer.ShipAddress,
                },
                Product = new ProductViewModel
                {
                    ProdCode = order.Product.ProdCode,
                    Name = order.Product.Name,
                }
            };

            return Ok(orderViewModel);
        }

        [HttpPut("{productProdCode}")]
        public async Task<ActionResult<OrderViewModel>> UpdateOrder(string productProdCode, OrderViewModel orderViewModel)
        {
            var order = await _context.Orders.FindAsync(productProdCode);

            if (order == null)
            {
                return NotFound();
            }

            // Update the order properties
            order.Qty = orderViewModel.Qty;
            order.Price = orderViewModel.Price;

            // You can update other properties as needed

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Map the updated order to OrderViewModel
            var updatedOrderViewModel = new OrderViewModel
            {
                // Map properties from the updated order entity
                OrderId = order.OrderId,
                OrderNo = order.OrderNo,
                OrderDate = order.OrderDate,
                CustomerCustCode = order.CustomerCustCode,
                ProductProdCode = order.ProductProdCode,
                Qty = order.Qty,
                Price = order.Price,
                // Map other properties as needed
            };

            // Return the updated order view model
            return Ok(updatedOrderViewModel);
        }

        [HttpPut("{rowNo}/{productCode}/{orderNo}")]
        public async Task<ActionResult<OrderViewModel>> UpdateOrder(int rowNo, string productCode, int orderNo, OrderViewModel updatedOrder)
        {
            try
            {
                // Find the order to update based on RowNo, ProductCode, and OrderNo
                var orderToUpdate = await _context.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Product)
                    .FirstOrDefaultAsync(o =>
                        o.RowNo == rowNo &&
                        o.ProductProdCode == productCode &&
                        o.OrderNo == orderNo);

                if (orderToUpdate == null)
                {
                    return NotFound($"Order with RowNo: {rowNo}, ProductCode: {productCode}, and OrderNo: {orderNo} not found.");
                }

                // Update the order properties with the new values
                orderToUpdate.Qty = updatedOrder.Qty;
                orderToUpdate.Price = updatedOrder.Price;

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return Ok($"Order with RowNo: {rowNo}, ProductCode: {productCode}, and OrderNo: {orderNo} updated successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return StatusCode(500, $"An error occurred while updating the order: {ex.Message}");
            }
        }

        [HttpDelete("{rowNo}/{productCode}/{orderNo}")]
        public async Task<ActionResult> DeleteOrder(int rowNo, string productCode, int orderNo)
        {
            try
            {
                // Find the order to delete based on RowNo, ProductCode, and OrderNo
                var orderToDelete = await _context.Orders
                    .FirstOrDefaultAsync(o =>
                        o.RowNo == rowNo &&
                        o.ProductProdCode == productCode &&
                        o.OrderNo == orderNo);

                if (orderToDelete == null)
                {
                    return NotFound($"Order with RowNo: {rowNo}, ProductCode: {productCode}, and OrderNo: {orderNo} not found.");
                }

                // Remove the order from the context
                _context.Orders.Remove(orderToDelete);

                // Save the changes to the database
                await _context.SaveChangesAsync();

                return Ok($"Order with RowNo: {rowNo}, ProductCode: {productCode}, and OrderNo: {orderNo} deleted successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                return StatusCode(500, $"An error occurred while deleting the order: {ex.Message}");
            }
        }
    }
}

    

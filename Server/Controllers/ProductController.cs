 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Server.Data;
using OrderingSystem.Shared;

namespace OrderingSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly OrderDataContext _context;

        public ProductController(OrderDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts(
           int page = 1,
           int pageSize = 10,
           string searchTerm = "")
        {
            try
            {
                Console.WriteLine($"Requesting products - Page: {page}, PageSize: {pageSize}, SearchTerm: {searchTerm}");

                var query = _context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        query = query.Where(p =>
                            p.Name.Contains(searchTerm) ||
                            p.ProdCode.Contains(searchTerm) ||
                            p.Price.ToString().Contains(searchTerm)
                        );
                    }
                }

                var totalItems = await query.CountAsync();

                var products = await query
                    .OrderByDescending(p => p.ProdCode)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var pagedList = new PagedList<Product>(products, totalItems, page, pageSize);

                return Ok(pagedList);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


        [HttpGet("{prodCode}")]
        public async Task<ActionResult<Product>> GetProductById(string prodCode)
        {
            var product = await _context.Products.FindAsync(prodCode);
            if (product == null)
            {
                return NotFound("Sorry, no hero here ");
            }
            return Ok(product);
        }



        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
           
            if (!ModelState.IsValid)
            {
                var errors = ModelState.SelectMany(v => v.Value.Errors);
                return BadRequest(new { Message = "Validation failed", Errors = errors });
            }

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return new ObjectResult(product)
            {
                StatusCode = 201,
                Value = product,
            };
        }

        [HttpPut("{prodCode}")]
        public async Task<ActionResult<List<Product>>> UpdateProduct(string prodCode, Product product)
        {
            var dbProduct = await _context.Products.FindAsync(prodCode);

            if (dbProduct == null)
            {
                return NotFound();
            }

         
            dbProduct.Name = product.Name;
           dbProduct.Price = product.Price;
           
            await _context.SaveChangesAsync();

            return Ok(dbProduct); 
        }

        [HttpDelete("{prodCode}")]
        public async Task<ActionResult<List<Product>>> DeleteProduct(string prodCode)
        {
            var product = await _context.Products.FindAsync(prodCode);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
    }
}

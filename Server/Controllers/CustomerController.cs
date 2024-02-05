using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Server.Data;
using OrderingSystem.Shared;

namespace OrderingSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly OrderDataContext _context;

        public CustomerController(OrderDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Customer>>> GetCustomers(
          int page = 1,
          int pageSize = 10,
          string searchTerm = "")
        {
            try
            {
                Console.WriteLine($"Requesting customers - Page: {page}, PageSize: {pageSize}, SearchTerm: {searchTerm}");

                var query = _context.Customers.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(c =>
                        c.Fname.Contains(searchTerm) ||
                        c.Lname.Contains(searchTerm) ||
                        c.Mname.Contains(searchTerm) ||
                        c.BillAddress.Contains(searchTerm) ||
                        c.ShipAddress.Contains(searchTerm) ||
                        c.Email.Contains(searchTerm) ||
                        c.CustCode.Contains(searchTerm) ||
                        c.HomeNum.Contains(searchTerm) ||
                        c.MobileNum.Contains(searchTerm)
                    );
                }

                var totalItems = await query.CountAsync();

                var customers = await query
                    .OrderBy(c => c.CustCode)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var pagedList = new PagedList<Customer>(customers, totalItems, page, pageSize);

                return Ok(pagedList);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{custCode}")]
        public async Task<ActionResult<Customer>> GetCustomerById(string custCode)
        {
            var customer = await _context.Customers.FindAsync(custCode);
            if (customer == null)
            {
                return NotFound("Sorry, no hero here ");
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> CreateCustomer(Customer customer)
        {
            
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();

            return new ObjectResult(customer)
            {
                StatusCode = 201,
                Value = customer,
            };
        }

        [HttpPut("{custCode}")]
        public async Task<ActionResult<List<Customer>>> UpdateCustomer(string custCode, Customer customer)
        {
            var dbCustomer = await _context.Customers.FindAsync(custCode);

            if (dbCustomer == null)
            {
                return NotFound();
            }
           
            dbCustomer.Fname = customer.Fname;
            dbCustomer.Mname = customer.Mname;
            dbCustomer.Lname = customer.Lname;
            dbCustomer.BillAddress = customer.BillAddress;
            dbCustomer.ShipAddress = customer.ShipAddress;
            dbCustomer.Email = customer.Email;
            dbCustomer.MobileNum = customer.MobileNum;
            dbCustomer.HomeNum = customer.HomeNum;
         
         
            await _context.SaveChangesAsync();

            return Ok(dbCustomer);
        }

        [HttpDelete("{custCode}")]
        public async Task<ActionResult<List<Customer>>> DeleteCustomer(string custCode)
        {
            var customer = await _context.Customers.FindAsync(custCode);

            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }
    }
}
    


using Microsoft.EntityFrameworkCore;
using OrderingSystem.Shared;

namespace OrderingSystem.Server.Data
{
    public class OrderDataContext : DbContext
    {
        public OrderDataContext(DbContextOptions<OrderDataContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }


     

    }
}



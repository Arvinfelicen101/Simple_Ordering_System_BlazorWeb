using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update;
using OrderingSystem.Shared;
using System.ComponentModel;
using System.Reflection;


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



using Intution_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Intution_API.Data
{
    public class APIDbContext : DbContext
    {

        public APIDbContext(DbContextOptions<APIDbContext> options ):base(options)
        {
            
        }


        public DbSet<Cart> Carts { get; set; }

        public DbSet<Product> Products { get; set; }


        public DbSet<Order> Orders { get; set; }

        public DbSet<ProductOrder> ProductOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductOrder>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<ProductOrder>()
                .HasOne(op => op.Order)
                .WithMany(o => o.ProductOrder)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(op => op.Product)
                .WithMany(p => p.ProductOrder)
                .HasForeignKey(op => op.ProductId);
        }

    }
}

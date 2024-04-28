using Microsoft.EntityFrameworkCore;

#nullable disable

namespace E_commerce_API.Domain.Models
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<SuccessfulOrder> SuccessfulOrders { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Cart>().ToTable("Carts");
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<SuccessfulOrder>().ToTable("Successful_Order");
            modelBuilder.Entity<Review>().ToTable("Review");
            modelBuilder.Entity<Location>().ToTable("Locations").HasNoKey();
        }
    }
}

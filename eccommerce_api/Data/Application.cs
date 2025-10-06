using eccommerce_api.model;
using Microsoft.EntityFrameworkCore;

namespace eccommerce_api.Data
{
    public class Application : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Api;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>().
                HasOne(p => p.Category).
                WithMany(p => p.Products).HasForeignKey(p=>p.CategoryId)
               .HasPrincipalKey(c=>c.Id);

            modelBuilder.Entity<User>().HasOne(u => u.Orders).WithMany(o => o.Users).HasForeignKey(u => u.OrderId);
        }

        public DbSet<Products>products { get; set; }
        public DbSet<Category>categories { get; set; }
        public DbSet<User>Users { get; set; }
        public DbSet<ProductItem>Items { get; set; }
        public DbSet<Order>Orders { get; set; }


    }
}

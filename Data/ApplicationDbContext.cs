using Microsoft.EntityFrameworkCore;
using Diploma.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Diploma.Data
{
    public class ApplicationDbContext : IdentityDbContext<RegisteredUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<RegisteredUser> RegisteredUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Repair> Repairs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Repair)
                .WithMany(r => r.Orders)
                .HasForeignKey(o => o.RepairId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Payment.API.Models
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Bank> Banks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Bank>().HasData(
                new Bank
                {
                    UserId = "1",
                    Balance = 3000m
                },
                new Bank
                {
                    UserId = "2",
                    Balance = 1000m
                }
            );
        }
    }
}
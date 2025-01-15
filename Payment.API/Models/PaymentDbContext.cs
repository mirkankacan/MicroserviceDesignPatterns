using Microsoft.EntityFrameworkCore;

namespace Payment.API.Models
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
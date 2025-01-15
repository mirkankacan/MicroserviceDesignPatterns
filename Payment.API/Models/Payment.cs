using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string Cvv { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}
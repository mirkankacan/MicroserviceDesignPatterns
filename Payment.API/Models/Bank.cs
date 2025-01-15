using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment.API.Models
{
    public class Bank
    {
        [Key]
        public string UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
    }
}
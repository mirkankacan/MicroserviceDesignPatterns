using MassTransit;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = default!;
        public int OrderId { get; set; } = default!;
        public string BuyerId { get; set; } = default!;
        public string CardName { get; set; } = default!;
        public string CardNumber { get; set; } = default!;
        public string Expiration { get; set; } = default!;
        public string Cvv { get; set; } = default!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; } = default!;

        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            var properties = GetType().GetProperties();
            var sb = new StringBuilder();
            properties.ToList().ForEach(p =>
            {
                var value = p.GetValue(this);
                sb.AppendLine($"{p.Name}: {value}");
            });
            sb.AppendLine("-----------------");
            return sb.ToString();
        }
    }
}
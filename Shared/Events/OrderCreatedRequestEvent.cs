using Shared.Interfaces;

namespace Shared.Events
{
    public class OrderCreatedRequestEvent : IOrderCreatedRequestEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
        public List<OrderItemMessage> OrderItems { get; set; } = new();
        public PaymentMessage Payment { get; set; }
    }
}
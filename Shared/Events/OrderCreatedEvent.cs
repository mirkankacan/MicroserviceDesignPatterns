using Shared.Interfaces;

namespace Shared.Events
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public List<OrderItemMessage> OrderItems { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
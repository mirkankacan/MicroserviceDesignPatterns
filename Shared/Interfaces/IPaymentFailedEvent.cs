using MassTransit;

namespace Shared.Interfaces
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        string Reason { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
    }
}
using MassTransit;

namespace Shared.Interfaces
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {
    }
}
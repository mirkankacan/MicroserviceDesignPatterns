using MassTransit;

namespace Shared.Interfaces
{
    public interface IStockReservedRequestPayment : CorrelatedBy<Guid>
    {
        PaymentMessage Payment { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }

        string BuyerId { get; set; }
    }
}
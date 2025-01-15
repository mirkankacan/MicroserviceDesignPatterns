namespace Shared
{
    public record PaymentCompletedEvent
    (
         int OrderId,
         string BuyerId
    );
}
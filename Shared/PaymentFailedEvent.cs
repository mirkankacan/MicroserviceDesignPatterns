namespace Shared
{
    public record PaymentFailedEvent
    (
        int OrderId,

        string BuyerId,

         string FailMessage
    );
}
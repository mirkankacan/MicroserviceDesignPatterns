namespace Shared
{
    public record OrderCreatedEvent
    (
        int OrderId,
        string BuyerId,
        PaymentMessage Payment,
        List<OrderItemMessage> OrderItems
    );
}
namespace Shared
{
    public record StockReservedEvent
    (
        int OrderId,
        string BuyerId,
        PaymentMessage Payment,
        List<OrderItemMessage> OrderItems
    );
}
namespace Shared
{
    public record StockReserverdEvent
    (
        int OrderId,
        string BuyerId,
        PaymentMessage Payment,
        List<OrderItemMessage> OrderItems
    );
}
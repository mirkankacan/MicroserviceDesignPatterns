namespace Shared
{
    public record StockNotReservedEvent
    (
        int OrderId,
        string FailMessage
    );
}
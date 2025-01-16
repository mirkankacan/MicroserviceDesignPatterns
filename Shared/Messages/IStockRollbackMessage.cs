namespace Shared.Messages
{
    public interface IStockRollbackMessage
    {
        int OrderId { get; set; }
        List<OrderItemMessage> OrderItems { get; set; }
    }
}
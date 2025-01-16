namespace Shared.Messages
{
    public class StockRollbackMessage : IStockRollbackMessage
    {
        public List<OrderItemMessage> OrderItems { get; set; }
        public int OrderId { get; set; }
    }
}
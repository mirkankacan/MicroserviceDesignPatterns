namespace EventSourcing.Shared.Events
{
    public sealed class ProductPriceChangedEvent
    {
        public Guid Id { get; set; }
        public decimal ChangedPrice { get; set; }
    }
}
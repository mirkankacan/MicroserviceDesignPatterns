namespace EventSourcing.Shared.Events
{
    public sealed class ProductPriceChangedEvent : IEvent
    {
        public Guid Id { get; set; }
        public decimal ChangedPrice { get; set; }
    }
}
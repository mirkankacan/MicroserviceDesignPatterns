namespace EventSourcing.Shared.Events
{
    public sealed class ProductDeletedEvent : IEvent
    {
        public Guid Id { get; set; }
    }
}
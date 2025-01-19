namespace EventSourcing.Shared.Events
{
    public sealed class ProductNameChangedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string ChangedName { get; set; }
    }
}
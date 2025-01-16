namespace Shared.Interfaces
{
    public interface IOrderRequestFailedEvent
    {
        int OrderId { get; set; }
        string Reason { get; set; }
    }
}
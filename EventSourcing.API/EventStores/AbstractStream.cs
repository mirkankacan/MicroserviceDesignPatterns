using EventSourcing.Shared.Events;
using EventStore.ClientAPI;
using System.Text;
using System.Text.Json;

namespace EventSourcing.API.EventStores
{
    public abstract class AbstractStream(string streamName, IEventStoreConnection eventStoreConnection)
    {
        protected readonly LinkedList<IEvent> Events = new LinkedList<IEvent>();

        public async Task SaveAsync()
        {
            var newEvents = Events.ToList().Select(x => new EventData(
                Guid.NewGuid(),
                x.GetType().Name,
                true,
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(x, inputType: x.GetType())),
                Encoding.UTF8.GetBytes(x.GetType().FullName)))
                .ToList();

            await eventStoreConnection.AppendToStreamAsync(streamName, ExpectedVersion.Any, newEvents);

            Events.Clear();
        }
    }
}
using EventSourcing.API.EventStores;
using EventStore.ClientAPI;
using System.Text;

namespace EventSourcing.API.BackgroundServices
{
    public class ProductReadModelEventStore(IEventStoreConnection eventStoreConnection, ILogger<ProductReadModelEventStore> logger, IServiceProvider serviceProvider) : BackgroundService
    {
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await eventStoreConnection.ConnectToPersistentSubscriptionAsync(ProductStream.StreamName, ProductStream.GroupName, EventAppeared);
            serviceProvider.CreateScope();
        }

        private async Task EventAppeared(EventStorePersistentSubscriptionBase subscription, ResolvedEvent resolvedEvent)
        {
            logger.LogInformation("The message is processing...");
            var type = Type.GetType($"{Encoding.UTF8.GetString(resolvedEvent.Event.Metadata)}, EventSourcing.Shared");
        }
    }
}
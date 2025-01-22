using EventSourcing.API.EventStores;
using EventSourcing.API.Models;
using EventSourcing.Shared.Events;
using EventStore.ClientAPI;
using System.Text;
using System.Text.Json;

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
            // autoAck Auto acknowledge messages processed
            await eventStoreConnection.ConnectToPersistentSubscriptionAsync(ProductStream.StreamName, ProductStream.GroupName, EventAppeared, autoAck: false);
            serviceProvider.CreateScope();
        }

        private async Task EventAppeared(EventStorePersistentSubscriptionBase subscription, ResolvedEvent resolvedEvent)
        {
            var type = Type.GetType($"{Encoding.UTF8.GetString(resolvedEvent.Event.Metadata)}, EventSourcing.Shared");
            logger.LogInformation($"The message is processing... : {type.ToString()}");

            var eventData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
            var @event = JsonSerializer.Deserialize(eventData, type);

            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Product? product = new();
            switch (@event)
            {
                case ProductCreatedEvent productCreatedEvent:
                    product = new()
                    {
                        Id = productCreatedEvent.Id,
                        Name = productCreatedEvent.Name,
                        Price = productCreatedEvent.Price,
                        Stock = productCreatedEvent.Stock,
                        UserId = productCreatedEvent.UserId
                    };
                    if (product is not null)
                        context.Products.Add(product);

                    break;

                case ProductNameChangedEvent productNameChangedEvent:
                    product = context.Products.Find(productNameChangedEvent.Id);
                    if (product is not null)
                        product.Name = productNameChangedEvent.ChangedName;

                    break;

                case ProductPriceChangedEvent productPriceChangedEvent:
                    product = context.Products.Find(productPriceChangedEvent.Id);
                    if (product is not null)
                        product.Price = productPriceChangedEvent.ChangedPrice;

                    break;

                case ProductDeletedEvent productDeletedEvent:
                    product = context.Products.Find(productDeletedEvent.Id);
                    if (product is not null)
                        context.Products.Remove(product);

                    break;
            }

            await context.SaveChangesAsync();
            subscription.Acknowledge(resolvedEvent.Event.EventId);
        }
    }
}
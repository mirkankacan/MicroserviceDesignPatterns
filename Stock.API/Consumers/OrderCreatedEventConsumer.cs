using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events;
using Shared.Interfaces;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer(StockDbContext dbContext, ILogger<OrderCreatedEventConsumer> logger, IPublishEndpoint publishEndpoint) : IConsumer<IOrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            var stockResult = new List<bool>();
            foreach (var item in context.Message.OrderItems)
            {
                stockResult.Add(await dbContext.Stocks.AnyAsync(x => x.ProductId == item.ProductId && x.Count > item.Count));
            }
            if (stockResult.All(x => x.Equals(true)))
            {
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await dbContext.Stocks.FirstOrDefaultAsync(x => x.ProductId == item.ProductId);
                    if (stock is not null)
                    {
                        stock.Count -= item.Count;
                    }
                }
                await dbContext.SaveChangesAsync();

                logger.LogInformation($"Stock was reserved for Correlation ID: {context.Message.CorrelationId}");

                StockReservedEvent stockReserverdEvent = new StockReservedEvent(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems
                };

                await publishEndpoint.Publish(stockReserverdEvent);
            }
            else
            {
                await publishEndpoint.Publish(new StockNotReservedEvent(context.Message.CorrelationId)
                {
                    Reason = "Not enough stock"
                });
                logger.LogWarning($"Not enough stock for Correlation ID: {context.Message.CorrelationId}");
            }
        }
    }
}
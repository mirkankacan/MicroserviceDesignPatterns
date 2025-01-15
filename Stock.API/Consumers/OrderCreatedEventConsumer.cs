using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer(StockDbContext dbContext, ILogger<OrderCreatedEventConsumer> logger, ISendEndpointProvider sendEndpoint, IPublishEndpoint publishEndpoint) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
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
                    if (stock != null)
                    {
                        stock.Count -= item.Count;
                    }
                }
                await dbContext.SaveChangesAsync();

                logger.LogInformation($"Stock was reserved for Buyer ID: {context.Message.BuyerId}");

                var send = await sendEndpoint.GetSendEndpoint(new Uri($"queue:{RabbitMqSettingsConst.StockReservedEventQueueName}"));

                StockReservedEvent stockReserverdEvent = new(context.Message.OrderId, context.Message.BuyerId, context.Message.Payment, context.Message.OrderItems);

                await send.Send(stockReserverdEvent);
            }
            else
            {

                await publishEndpoint.Publish(new StockNotReservedEvent(context.Message.OrderId, "Not enough stock"));
                logger.LogWarning($"Not enough stock for Order ID: {context.Message.OrderId}");

            }
        }
    }
}
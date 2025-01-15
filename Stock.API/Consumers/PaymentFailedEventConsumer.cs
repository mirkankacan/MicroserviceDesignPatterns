using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class PaymentFailedEventConsumer(StockDbContext dbContext, ILogger<PaymentFailedEventConsumer> logger, ISendEndpointProvider sendEndpoint, IPublishEndpoint publishEndpoint) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == item.ProductId);
                if (stock is not null)
                {
                    stock.Count += item.Count;
                }
                else
                {
                    logger.LogWarning($"Stock (ID {item.ProductId}) not found");
                }
            }
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Stock was released for Order ID: {context.Message.OrderId}");
        }
    }
}
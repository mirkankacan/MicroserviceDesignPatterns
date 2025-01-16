using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using Stock.API.Models;

namespace Stock.API.Consumers
{
    public class PaymentFailedEventConsumer(StockDbContext dbContext, ILogger<PaymentFailedEventConsumer> logger, ISendEndpointProvider sendEndpoint, IPublishEndpoint publishEndpoint) : IConsumer<IPaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<IPaymentFailedEvent> context)
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
            logger.LogInformation($"Stock was released for Correlation ID: {context.Message.CorrelationId}");
        }
    }
}
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer(OrderDbContext dbContext, ILogger<StockNotReservedEventConsumer> logger) : IConsumer<StockNotReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
            if (order is not null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = context.Message.FailMessage;
                await dbContext.SaveChangesAsync();
                logger.LogWarning($"Order (Id={context.Message.OrderId}) status changed : {order.Status} reason: {context.Message.FailMessage}");
            }
            else
            {
                logger.LogError($"Order (Id={context.Message.OrderId} not found");
            }
        }
    }
}
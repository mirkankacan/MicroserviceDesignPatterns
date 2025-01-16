using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Interfaces;

namespace Order.API.Consumers
{
    public class OrderRequestFailedEventConsumer(OrderDbContext dbContext, ILogger<OrderRequestFailedEventConsumer> logger) : IConsumer<IOrderRequestFailedEvent>
    {
        public async Task Consume(ConsumeContext<IOrderRequestFailedEvent> context)
        {
            var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
            if (order is not null)
            {
                order.Status = OrderStatus.Fail;
                order.FailMessage = context.Message.Reason;
                await dbContext.SaveChangesAsync();
                logger.LogWarning($"Order (Id={context.Message.OrderId}) status changed : {order.Status} reason: {context.Message.Reason}");
            }
            else
            {
                logger.LogError($"Order (Id={context.Message.OrderId} not found");
            }
        }
    }
}
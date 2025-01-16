using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Interfaces;

namespace Order.API.Consumers
{
    public class OrderRequestCompletedEventConsumer(OrderDbContext dbContext, ILogger<OrderRequestCompletedEventConsumer> logger) : IConsumer<IOrderRequestCompletedEvent>
    {
        public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
            if (order is not null)
            {
                order.Status = OrderStatus.Completed;
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Order (Id={context.Message.OrderId}) status changed : {order.Status}");
            }
            else
            {
                logger.LogError($"Order (Id={context.Message.OrderId}) not found");
            }
        }
    }
}
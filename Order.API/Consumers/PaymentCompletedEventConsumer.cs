using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer(OrderDbContext dbContext, ILogger<PaymentCompletedEventConsumer> logger) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == context.Message.OrderId);
            if (order is not null)
            {
                order.Status = OrderStatus.Completed;
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Order (Id={context.Message.OrderId} status changed : {order.Status}");
            }
            else
            {
                logger.LogError($"Order (Id={context.Message.OrderId} not found");
            }
        }
    }
}
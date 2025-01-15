using MassTransit;
using Microsoft.EntityFrameworkCore;
using Payment.API.Models;
using Shared;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer(PaymentDbContext dbContext, IPublishEndpoint publishEndpoint, ILogger<StockReservedEventConsumer> logger) : IConsumer<StockReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var userBank = await dbContext.Banks.FirstOrDefaultAsync(x => x.UserId == context.Message.BuyerId);
            if (userBank is not null && userBank.Balance > context.Message.Payment.TotalPrice)
            {
                userBank.Balance -= context.Message.Payment.TotalPrice;

                var newPayment = new Models.Payment()
                {
                    CardName = context.Message.Payment.CardName,
                    CardNumber = context.Message.Payment.CardNumber,
                    Expiration = context.Message.Payment.Expiration,
                    Cvv = context.Message.Payment.Cvv,
                    TotalPrice = context.Message.Payment.TotalPrice,
                };
                await dbContext.Payments.AddAsync(newPayment);

                await dbContext.SaveChangesAsync();

                logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit cart for Buyer ID: {context.Message.BuyerId} Remaining Balance: {userBank.Balance}");
                await publishEndpoint.Publish(new PaymentCompletedEvent(context.Message.OrderId, context.Message.BuyerId));
            }
            else
            {
                logger.LogWarning($"{context.Message.Payment.TotalPrice} TL wasn't withdrawn from credit cart for Buyer ID: {context.Message.BuyerId}");

                await publishEndpoint.Publish(new PaymentFailedEvent(context.Message.OrderId, context.Message.BuyerId, "Not enough balance", context.Message.OrderItems));
            }
        }
    }
}
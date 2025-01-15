using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.Dtos;
using Order.API.Models;
using Shared;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(OrderDbContext dbContext, IPublishEndpoint publishEndpoint) : ControllerBase
    {
        [HttpPost]
        public async Task<IResult> CreateOrder(OrderCreateDto orderCreate)
        {
            var newOrder = new Models.Order()
            {
                BuyerId = orderCreate.BuyerId,
                Status = OrderStatus.Suspend,
                Address = new Address() { District = orderCreate.Address.District, Line = orderCreate.Address.Line, Province = orderCreate.Address.Province },
                CreatedDate = DateTime.Now,
            };
            orderCreate.OrderItems.ForEach(item =>
            {
                newOrder.Items.Add(new OrderItem() { ProductId = item.ProductId, Price = item.Price, Count = item.Count });
            });

            await dbContext.Orders.AddAsync(newOrder);
            await dbContext.SaveChangesAsync();

            var orderCreatedEvent = new OrderCreatedEvent(
                newOrder.Id,
                orderCreate.BuyerId,
                new PaymentMessage(
                    orderCreate.Payment.CardName,
                    orderCreate.Payment.CardNumber,
                    orderCreate.Payment.Expiration,
                    orderCreate.Payment.Cvv,
                    orderCreate.OrderItems.Sum(x => x.Price * x.Count)),
               new List<OrderItemMessage>());

            orderCreate.OrderItems.ForEach(item =>
            {
                orderCreatedEvent.OrderItems.Add(new OrderItemMessage
                (
                    item.ProductId,
                    item.Count
                ));
            });

            // Goes to Exchange. you need to subscribe to get the published data. Used when there is more than one listener.
            // Send goes directly to the queue. Used when there is only one listener.
            await publishEndpoint.Publish(orderCreatedEvent);

            return Results.Ok();
        }
    }
}
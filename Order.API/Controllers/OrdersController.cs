using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Order.API.Dtos;
using Order.API.Models;
using Shared;
using Shared.Events;
using Shared.Interfaces;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(OrderDbContext dbContext, ISendEndpointProvider sendEndpoint) : ControllerBase
    {
        [HttpGet]
        public async Task<IResult> GetOrders()
        {
            return Results.Ok(await dbContext.Orders.ToListAsync());
        }

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

            await dbContext.AddAsync(newOrder);
            await dbContext.SaveChangesAsync();

            var orderCreatedRequestEvent = new OrderCreatedRequestEvent()
            {
                BuyerId = orderCreate.BuyerId,
                OrderId = newOrder.Id,
                Payment = new PaymentMessage(
                    orderCreate.Payment.CardName,
                    orderCreate.Payment.CardNumber,
                    orderCreate.Payment.Expiration,
                    orderCreate.Payment.Cvv,
                    orderCreate.OrderItems.Sum(x => x.Price * x.Count)),
            };

            orderCreate.OrderItems.ForEach(item =>
            {
                orderCreatedRequestEvent.OrderItems.Add(new OrderItemMessage(item.ProductId, item.Count));
            });

            // Goes to Exchange. you need to subscribe to get the published data. Used when there is more than one listener.
            // Send goes directly to the queue. Used when there is only one listener.

            var send = await sendEndpoint.GetSendEndpoint(new Uri($"queue:{RabbitMqSettingsConst.OrderSaga}"));

            await send.Send<IOrderCreatedRequestEvent>(orderCreatedRequestEvent);

            return Results.Created();
        }
    }
}
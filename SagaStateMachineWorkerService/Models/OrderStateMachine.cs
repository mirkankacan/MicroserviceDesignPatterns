using MassTransit;
using Shared;
using Shared.Events;
using Shared.Interfaces;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public Event<IStockReservedEvent> StockReservedEvent { get; set; }
        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }
        public State OrderCreated { get; private set; }
        public State StockReserved { get; private set; }
        public State PaymentCompleted { get; private set; }

        public OrderStateMachine()
        {
            // Initial stage
            InstanceState(x => x.CurrentState);

            // Compare the OrderId in the database with the OrderId from the event, if there is none, create a new row CorrelationId assign a random guid
            Event(() => OrderCreatedRequestEvent, x => x.CorrelateBy<int>(y => y.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));
            Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
            Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Initially(When(OrderCreatedRequestEvent)
            .Then(context =>
            {
                Console.WriteLine($"Initial : {context.Saga.CurrentState.ToString()}");

                context.Saga.OrderId = context.Message.OrderId;
                context.Saga.BuyerId = context.Message.BuyerId;
                context.Saga.CreatedDate = DateTime.Now;

                context.Saga.CardName = context.Message.Payment.CardName;
                context.Saga.CardNumber = context.Message.Payment.CardNumber;
                context.Saga.Cvv = context.Message.Payment.Cvv;
                context.Saga.Expiration = context.Message.Payment.Expiration;
                context.Saga.TotalPrice = context.Message.Payment.TotalPrice;
            })
            .Then(context =>
            {
                Console.WriteLine($"OrderCreatedRequestEvent before : {context.Saga.ToString()}");
            })
            .PublishAsync(context => context.Init<OrderCreatedEvent>(new OrderCreatedEvent()
            {
                CorrelationId = context.Saga.CorrelationId,
                OrderItems = context.Message.OrderItems
            }))
            .TransitionTo(OrderCreated)
            .Then(context =>
            {
                Console.WriteLine($"OrderCreatedRequestEvent after : {context.Saga.ToString()}");
            }));

            During(OrderCreated,
                When(StockReservedEvent)
                .TransitionTo(StockReserved)
                .SendAsync(new Uri($"queue:{RabbitMqSettingsConst.PaymentStockReservedRequestQueueName}"), context => context.Init<StockReservedRequestPayment>(new StockReservedRequestPayment()
                {
                    CorrelationId = context.Saga.CorrelationId,
                    OrderItems = context.Message.OrderItems,
                    BuyerId = context.Saga.BuyerId,
                    Payment = new PaymentMessage(context.Saga.CardName, context.Saga.CardNumber, context.Saga.Expiration, context.Saga.Cvv, context.Saga.TotalPrice)
                }))
                .Then(context =>
                {
                    Console.WriteLine($"StockReservedEvent after : {context.Saga.ToString()}");
                }));

            During(StockReserved,
                When(PaymentCompletedEvent)
                .TransitionTo(PaymentCompleted)
                .PublishAsync(context => context.Init<OrderRequestCompletedEvent>(new OrderRequestCompletedEvent()
                {
                    OrderId = context.Saga.OrderId
                }))
                .Then(context =>
                {
                    Console.WriteLine($"PaymentCompleted after : {context.Saga.ToString()}");
                })
                .Finalize());
        }
    }
}
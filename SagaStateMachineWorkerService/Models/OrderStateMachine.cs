using MassTransit;
using Shared.Interfaces;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }
        public State OrderCreated { get; private set; }

        public OrderStateMachine()
        {
            // Initial stage
            InstanceState(x => x.CurrentState);

            // Compare the OrderId in the database with the OrderId from the event, if there is none, create a new row CorrelationId assign a random guid
            Event(() => OrderCreatedRequestEvent, y => y.CorrelateBy<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Initially(When(OrderCreatedRequestEvent).Then(context =>
            {
                context.Saga.OrderId = context.Message.OrderId;
                context.Saga.BuyerId = context.Message.BuyerId;
                context.Saga.CreatedDate = DateTime.Now;

                context.Saga.CardName = context.Message.Payment.CardName;
                context.Saga.CardNumber = context.Message.Payment.CardNumber;
                context.Saga.Cvv = context.Message.Payment.Cvv;
                context.Saga.Expiration = context.Message.Payment.Expiration;
                context.Saga.TotalPrice = context.Message.Payment.TotalPrice;
            }).Then(context =>
            {
                Console.WriteLine($"OrderCreatedRequestEvent before : {context.Saga.ToString()}");
            })
            .TransitionTo(OrderCreated).Then(context =>
            {
                Console.WriteLine($"OrderCreatedRequestEvent after : {context.Saga.ToString()}");
            }));
        }
    }
}
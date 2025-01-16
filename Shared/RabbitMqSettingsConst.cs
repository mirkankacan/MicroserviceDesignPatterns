namespace Shared
{
    public class RabbitMqSettingsConst
    {
        public const string OrderSaga = "order-saga-queue";
        public const string PaymentStockReservedRequestQueueName = "payment-stock-reserved-request-queue";
        public const string OrderRequestCompletedEventQueueName = "order-request-completed-queue";
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
    }
}
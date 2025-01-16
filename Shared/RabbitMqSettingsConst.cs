namespace Shared
{
    public class RabbitMqSettingsConst
    {
        public const string OrderSaga = "order-saga-queue";

        // Choreography
        // Stock
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";

        public const string StockReservedEventQueueName = "stock-reserved-queue";
        public const string StockNotReservedEventQueueName = "stock-not-reserved-queue";
        public const string StockPaymentFailedEventQueueName = "stock-payment-failed-queue";

        // Order
        public const string OrderPaymentCompletedEventQueueName = "order-payment-completed-queue";

        public const string OrderPaymentFailedEventQueueName = "order-payment-failed-queue";
    }
}
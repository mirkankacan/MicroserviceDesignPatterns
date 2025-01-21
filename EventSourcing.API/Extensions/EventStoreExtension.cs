using EventStore.ClientAPI;

namespace EventSourcing.API.Extensions
{
    public static class EventStoreExtension
    {
        public static void AddEventStore(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionSettings = ConnectionSettings.Create().KeepReconnecting().KeepRetrying().DisableServerCertificateValidation().DisableTls().Build();
            var connection = EventStoreConnection.Create(connectionSettings, new Uri(configuration.GetConnectionString("EventStore")!));
            connection.ConnectAsync().Wait();

            services.AddSingleton(connection);

            using var logFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
            });
            var logger = logFactory.CreateLogger("Startup");
            connection.Connected += (sender, args) =>
            {
                logger.LogInformation("EventStore connection established");
            };
            connection.ErrorOccurred += (sender, args) =>
            {
                logger.LogError(args.Exception.Message);
            };
            connection.AuthenticationFailed += (sender, args) =>
            {
                logger.LogError(args.Reason);
            };
            connection.Reconnecting += (sender, args) =>
            {
                logger.LogInformation($"EventStore reconnecting");
            };
            connection.Disconnected += (sender, args) =>
            {
                logger.LogInformation($"EventStore disconnected");
            };
        }
    }
}
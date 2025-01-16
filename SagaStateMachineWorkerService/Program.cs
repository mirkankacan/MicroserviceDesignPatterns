using MassTransit;
using Microsoft.EntityFrameworkCore;
using SagaStateMachineWorkerService;
using SagaStateMachineWorkerService.Models;
using Shared;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddSagaStateMachine<OrderStateMachine, OrderStateInstance>()
    .EntityFrameworkRepository(opts =>
    {
        opts.AddDbContext<DbContext, OrderStateDbContext>((provider, context) =>
        {
            context.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")!, m =>
            {
                m.MigrationsAssembly(assemblyName: Assembly.GetExecutingAssembly().GetName().Name);
            });
        });
    });

    x.UsingRabbitMq((provider, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), host =>
        {
            host.Username(builder.Configuration["MessageBroker:UserName"]!);
            host.Password(builder.Configuration["MessageBroker:Password"]!);
        });
        cfg.ReceiveEndpoint(RabbitMqSettingsConst.OrderSaga, e =>
        {
            e.ConfigureSaga<OrderStateInstance>(provider);
        });
    });
});
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
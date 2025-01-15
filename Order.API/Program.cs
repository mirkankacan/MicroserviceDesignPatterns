using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Consumers;
using Order.API.Models;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumer<PaymentCompletedEventConsumer>();
    x.AddConsumer<PaymentFailedEventConsumer>();
    x.AddConsumer<StockNotReservedEventConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), host =>
        {
            host.Username(builder.Configuration["MessageBroker:UserName"]!);
            host.Password(builder.Configuration["MessageBroker:Password"]!);
        });
        cfg.ReceiveEndpoint(RabbitMqSettingsConst.OrderPaymentCompletedEventQueueName, e =>
        {
            e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
        });
        cfg.ReceiveEndpoint(RabbitMqSettingsConst.OrderPaymentFailedEventQueueName, e =>
        {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });
        cfg.ReceiveEndpoint(RabbitMqSettingsConst.StockNotReservedEventQueueName, e =>
        {
            e.ConfigureConsumer<StockNotReservedEventConsumer>(context);
        });
    });
});

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapControllers();

app.UseHttpsRedirection();

app.Run();
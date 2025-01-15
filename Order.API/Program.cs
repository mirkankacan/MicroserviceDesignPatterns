using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;

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
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), host =>
        {
            host.Username(builder.Configuration["MessageBroker:UserName"]!);
            host.Password(builder.Configuration["MessageBroker:Password"]!);
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
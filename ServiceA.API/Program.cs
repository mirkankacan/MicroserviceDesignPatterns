using Polly;
using Polly.Extensions.Http;
using ServiceA.API.Services;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

static IAsyncPolicy<HttpResponseMessage> GetWaitAndRetryPolicy()
{
    return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
                        {
                            Console.WriteLine($"Retry {retryCount} after {timeSpan.TotalSeconds}s due to: {exception.Exception.Message}");
                        });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
                          .HandleTransientHttpError()
                          .CircuitBreakerAsync(3, TimeSpan.FromSeconds(10), onBreak: (delegateResult, timeSpan) =>
                          {
                              Console.WriteLine("Circuit Breaker status => On Break");
                          },
                          onReset: () =>
                          {
                              Console.WriteLine("Circuit Breaker status => On Reset");
                          },
                          onHalfOpen: () =>
                          {
                              Console.WriteLine("Circuit Breaker Status => On Half Open");
                          });
}
static IAsyncPolicy<HttpResponseMessage> GetAdvancedCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
                          .HandleTransientHttpError()

                          // If the ratio of failed requests within 30 seconds is 5 in 10 and there are 30 failed requests then wait 30 secs to process requests
                          .AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(30), 30, TimeSpan.FromSeconds(30), onBreak: (delegateResult, timeSpan) =>
                          {
                              Console.WriteLine("Circuit Breaker status => On Break");
                          },
                          onReset: () =>
                          {
                              Console.WriteLine("Circuit Breaker status => On Reset");
                          },
                          onHalfOpen: () =>
                          {
                              Console.WriteLine("Circuit Breaker Status => On Half Open");
                          });
}

builder.Services.AddHttpClient("ServiceB", opts =>
{
    opts.BaseAddress = new Uri("http://localhost:5000/api/products/");
    opts.DefaultRequestHeaders.Clear();
    opts.DefaultRequestHeaders.Add("Accept", "application/json");
}).AddPolicyHandler(GetAdvancedCircuitBreakerPolicy());

builder.Services.AddScoped<ProductService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
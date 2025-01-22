using ServiceA.API.Models;

namespace ServiceA.API.Services
{
    public class ProductService(HttpClient httpClient, IHttpClientFactory clientFactory, ILogger<ProductService> logger)
    {
        public async Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            httpClient = clientFactory.CreateClient("ServiceB");
            Product? response = await httpClient.GetFromJsonAsync<Product>($"{id}", cancellationToken);
            if (response is null)
                logger.LogWarning($"Product not found in ServiceB.API for id: {id}");

            logger.LogInformation($"Product retrieved from ServiceB.API: {response}");
            return response;
        }
    }
}
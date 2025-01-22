using Microsoft.AspNetCore.Mvc;
using ServiceA.API.Services;

namespace ServiceA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(ProductService productService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductAsync(int id)
        {
            var product = await productService.GetProductByIdAsync(id);
            return Ok(product);
        }
    }
}
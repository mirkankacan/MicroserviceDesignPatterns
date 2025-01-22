using EventSourcing.API.Command;
using EventSourcing.API.Dtos;
using EventSourcing.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var values = await mediator.Send(new GetProductsQuery());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var value = await mediator.Send(new GetProductByIdQuery(id));
            return Ok(value);
        }

        [HttpGet("{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetProductsPaginated(int pageIndex, int pageSize)
        {
            var values = await mediator.Send(new GetProductsPaginatedQuery(pageIndex, pageSize));
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            await mediator.Send(new CreateProductCommand()
            {
                CreateProductDto = createProductDto
            });
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProductName(ChangeProductNameDto changeProductNameDto)
        {
            await mediator.Send(new ChangeProductNameCommand()
            {
                ChangeProductNameDto = changeProductNameDto
            });
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> ChangeProductPrice(ChangeProductPriceDto changeProductPriceDto)
        {
            await mediator.Send(new ChangeProductPriceCommand()
            {
                ChangeProductPriceDto = changeProductPriceDto
            });
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            await mediator.Send(new DeleteProductCommand()
            {
                Id = id
            });
            return NoContent();
        }
    }
}
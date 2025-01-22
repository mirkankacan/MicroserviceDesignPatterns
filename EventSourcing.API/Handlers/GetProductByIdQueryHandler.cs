using EventSourcing.API.Dtos;
using EventSourcing.API.Models;
using EventSourcing.API.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.API.Handlers
{
    public class GetProductByIdQueryHandler(AppDbContext dbContext) : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);
            var productDto = new ProductDto(product.Id, product.Name, product.Stock, product.Price, product.UserId);
            return productDto;
        }
    }
}
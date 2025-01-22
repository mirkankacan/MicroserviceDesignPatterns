using EventSourcing.API.Dtos;
using EventSourcing.API.Models;
using EventSourcing.API.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.API.Handlers
{
    public class GetProductsQueryHandler(AppDbContext dbContext) : IRequestHandler<GetProductsQuery, List<ProductDto>>
    {
        public async Task<List<ProductDto>> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {
            var products = await dbContext.Products.ToListAsync(cancellationToken);
            var productDtoAsList = products.Select(x => new ProductDto(x.Id, x.Name, x.Stock, x.Price, x.UserId)).ToList();

            return productDtoAsList;
        }
    }
}
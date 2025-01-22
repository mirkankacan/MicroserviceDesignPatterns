using EventSourcing.API.Dtos;
using EventSourcing.API.Models;
using EventSourcing.API.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.API.Handlers
{
    public class GetProductsPaginatedQueryHandler(AppDbContext dbContext) : IRequestHandler<GetProductsPaginatedQuery, ProductPaginatedDto>
    {
        public async Task<ProductPaginatedDto> Handle(GetProductsPaginatedQuery query, CancellationToken cancellationToken)
        {
            var products = await dbContext.Products
                .Skip(query.PageIndex * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            var paginatedProducts = new ProductPaginatedDto(query.PageIndex, query.PageSize, await dbContext.Products.CountAsync(cancellationToken), products);

            return paginatedProducts;
        }
    }
}
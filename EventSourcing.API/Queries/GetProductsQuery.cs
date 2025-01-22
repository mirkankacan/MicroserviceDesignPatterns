using EventSourcing.API.Dtos;
using MediatR;

namespace EventSourcing.API.Queries
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {
    }
}
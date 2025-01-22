using EventSourcing.API.Dtos;
using MediatR;

namespace EventSourcing.API.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public GetProductByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id
        {
            get;
        }
    }
}
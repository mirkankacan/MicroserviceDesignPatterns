using EventSourcing.API.Command;
using EventSourcing.API.EventStores;
using MediatR;

namespace EventSourcing.API.Handlers
{
    public class CreateProductCommandHandler(ProductStream productStream) : IRequestHandler<CreateProductCommand, Unit>
    {
        public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            productStream.Created(request.CreateProductDto);
            await productStream.SaveAsync();

            // If it is necessary to return a value Unit.Value can be used
            return Unit.Value;
        }
    }
}
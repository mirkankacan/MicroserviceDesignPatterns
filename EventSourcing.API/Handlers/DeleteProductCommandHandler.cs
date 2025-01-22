using EventSourcing.API.Command;
using EventSourcing.API.EventStores;
using MediatR;

namespace EventSourcing.API.Handlers
{
    public class DeleteProductCommandHandler(ProductStream productStream) : IRequestHandler<DeleteProductCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            productStream.Deleted(command.Id);
            await productStream.SaveAsync();
            return Unit.Value;
        }
    }
}
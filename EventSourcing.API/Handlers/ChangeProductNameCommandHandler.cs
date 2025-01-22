using EventSourcing.API.Command;
using EventSourcing.API.EventStores;
using MediatR;

namespace EventSourcing.API.Handlers
{
    public class ChangeProductNameCommandHandler(ProductStream productStream) : IRequestHandler<ChangeProductNameCommand, Unit>
    {
        public async Task<Unit> Handle(ChangeProductNameCommand command, CancellationToken cancellationToken)
        {
            productStream.NameChanged(command.ChangeProductNameDto);
            await productStream.SaveAsync();
            return Unit.Value;
        }
    }
}
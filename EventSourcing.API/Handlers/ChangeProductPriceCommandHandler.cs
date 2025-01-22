using EventSourcing.API.Command;
using EventSourcing.API.EventStores;
using MediatR;

namespace EventSourcing.API.Handlers
{
    public class ChangeProductPriceCommandHandler(ProductStream productStream) : IRequestHandler<ChangeProductPriceCommand, Unit>
    {
        public async Task<Unit> Handle(ChangeProductPriceCommand command, CancellationToken cancellationToken)
        {
            productStream.PriceChanged(command.ChangeProductPriceDto);
            await productStream.SaveAsync();
            return Unit.Value;
        }
    }
}
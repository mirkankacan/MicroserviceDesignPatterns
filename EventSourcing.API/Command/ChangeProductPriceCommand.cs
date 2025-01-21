using EventSourcing.API.Dtos;
using MediatR;

namespace EventSourcing.API.Command
{
    public class ChangeProductPriceCommand : IRequest<Unit>
    {
        public ChangeProductPriceDto ChangeProductPriceDto { get; set; }
    }
}
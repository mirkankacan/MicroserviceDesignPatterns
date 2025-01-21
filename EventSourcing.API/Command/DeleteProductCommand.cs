using MediatR;

namespace EventSourcing.API.Command
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
namespace EventSourcing.API.Dtos
{
    public sealed record ProductDto(Guid Id, string Name, int Stock, decimal Price, int UserId);
}
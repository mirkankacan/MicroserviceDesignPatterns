namespace EventSourcing.API.Dtos
{
    public sealed record CreateProductDto(string Name, int Stock, decimal Price, int UserId);
}
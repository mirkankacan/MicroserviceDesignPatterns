namespace EventSourcing.API.Dtos
{
    public sealed record ChangeProductPriceDto(Guid Id, decimal Price);
}
namespace Order.API.Dtos
{
    public record OrderItemDto
    (
        int ProductId,
        int Count,
        decimal Price
    );
}
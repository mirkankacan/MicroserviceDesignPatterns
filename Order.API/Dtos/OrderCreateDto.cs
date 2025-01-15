namespace Order.API.Dtos
{
    public record OrderCreateDto
    (
        string BuyerId,
        List<OrderItemDto> OrderItems,
        PaymentDto Payment,
        AddressDto Address
    );
}
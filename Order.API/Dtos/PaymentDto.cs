namespace Order.API.Dtos
{
    public record PaymentDto
    (
          string CardName,
         string CardNumber,
         string Expiration,
         string Cvv
    );
}
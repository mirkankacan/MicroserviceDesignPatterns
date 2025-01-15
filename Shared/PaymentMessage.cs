namespace Shared
{
    public record PaymentMessage
    (
          string CardName,
         string CardNumber,
         string Expiration,
         string Cvv,
         decimal TotalPrice
    );
}
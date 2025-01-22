using EventSourcing.API.Models;

namespace EventSourcing.API.Dtos
{
    public sealed record ProductPaginatedDto(int PageIndex, int PageSize, long Count, List<Product> Data);
}
namespace JordiAragonZaragoza.SharedKernel.Application.Contracts
{
    using System.Collections.Generic;

    public record class PaginatedCollectionOutputDto<T>(
        int ActualPage,
        int TotalPages,
        int TotalItems,
        IEnumerable<T> Items);
}
namespace JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts
{
    using System.Collections.Generic;

    public record class PaginatedCollectionResponse<T>(
        int ActualPage,
        int TotalPages,
        int TotalItems,
        IEnumerable<T> Items);
}
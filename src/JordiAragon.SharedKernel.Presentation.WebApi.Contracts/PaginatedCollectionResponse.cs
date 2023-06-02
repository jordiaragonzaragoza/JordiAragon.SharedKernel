namespace JordiAragon.SharedKernel.Presentation.WebApi.Contracts
{
    using System.Collections.Generic;

    public record class PaginatedCollectionResponse<T>(
        int ActualPage,
        int TotalPages,
        int TotalItems,
        IEnumerable<T> Items);
}
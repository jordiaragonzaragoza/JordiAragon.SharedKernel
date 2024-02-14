namespace JordiAragon.SharedKernel.Presentation.WebApi.Contracts
{
    using System.ComponentModel;

    public abstract record class PaginatedRequest
    {
        [DefaultValue(1)]
        public int? PageNumber { get; init; }

        [DefaultValue(10)]
        public int? PageSize { get; init; }
    }
}
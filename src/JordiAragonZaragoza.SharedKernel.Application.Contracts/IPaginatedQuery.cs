namespace JordiAragonZaragoza.SharedKernel.Application.Contracts
{
    public interface IPaginatedQuery
    {
        int PageNumber { get; }

        int PageSize { get; }
    }
}
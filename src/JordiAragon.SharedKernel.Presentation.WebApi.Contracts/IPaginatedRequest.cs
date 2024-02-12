namespace JordiAragon.SharedKernel.Presentation.WebApi.Contracts
{
    public interface IPaginatedRequest
    {
        int PageNumber { get; }

        int PageSize { get; }
    }
}
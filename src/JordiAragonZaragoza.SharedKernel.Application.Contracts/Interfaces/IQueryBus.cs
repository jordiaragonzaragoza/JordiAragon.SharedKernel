namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;

    public interface IQueryBus
    {
        Task<Result<TResponse>> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);
    }
}
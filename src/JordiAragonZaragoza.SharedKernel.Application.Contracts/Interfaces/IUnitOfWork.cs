namespace JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.Result;

    public interface IUnitOfWork
    {
        Task<TResponse> ExecuteInTransactionAsync<TResponse>(Func<Task<TResponse>> operation)
            where TResponse : IResult;
    }
}
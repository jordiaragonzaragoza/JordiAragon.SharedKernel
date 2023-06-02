namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Result;
    using MediatR;

    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
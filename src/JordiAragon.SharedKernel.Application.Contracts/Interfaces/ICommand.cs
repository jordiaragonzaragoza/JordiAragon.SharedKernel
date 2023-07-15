namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Result;
    using MediatR;

    // Using pure DDD, Commands will not use response with a Result<Response> check <see cref="ICommand"/>
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
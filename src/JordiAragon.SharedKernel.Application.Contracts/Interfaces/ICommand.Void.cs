namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using Ardalis.Result;
    using MediatR;

    public interface ICommand : IRequest<Result>
    {
    }
}
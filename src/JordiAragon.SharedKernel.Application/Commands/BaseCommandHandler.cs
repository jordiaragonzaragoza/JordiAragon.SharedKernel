namespace JordiAragon.SharedKernel.Application.Commands
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public abstract class BaseCommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public IEnumerable<IApplicationEvent> ApplicationEvents { get; } = new List<IApplicationEvent>();

        public abstract Task<Result<TResponse>> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
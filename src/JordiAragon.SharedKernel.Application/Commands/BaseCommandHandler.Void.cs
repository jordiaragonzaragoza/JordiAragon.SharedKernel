namespace JordiAragon.SharedKernel.Application.Commands
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public abstract class BaseCommandHandler<TCommand> : ICommandHandler<TCommand>
         where TCommand : ICommand
    {
        public IEnumerable<IApplicationEvent> ApplicationEvents { get; } = new List<IApplicationEvent>();

        public abstract Task<Result> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
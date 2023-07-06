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
        private readonly List<IApplicationEvent> applicationEvents = new();

        public IEnumerable<IApplicationEvent> ApplicationEvents => this.applicationEvents.AsReadOnly();

        public abstract Task<Result> Handle(TCommand request, CancellationToken cancellationToken);

        public void ClearApplicationEvents() => this.applicationEvents.Clear();

        protected void RegisterApplicationEvent(IApplicationEvent applicationEvent) => this.applicationEvents.Add(applicationEvent);
    }
}
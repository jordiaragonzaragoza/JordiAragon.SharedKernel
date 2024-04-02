namespace JordiAragon.SharedKernel.Application.Commands.Decorators
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Domain.Events.Services;

    public class CommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> decorated;
        private readonly IEventsDispatcherService eventsDispatcherService;

        public CommandHandlerDecorator(
            IEventsDispatcherService eventsDispatcherService,
            ICommandHandler<TCommand> decorated)
        {
            this.eventsDispatcherService = Guard.Against.Null(eventsDispatcherService, nameof(eventsDispatcherService));
            this.decorated = Guard.Against.Null(decorated, nameof(decorated));
            this.Events = decorated.Events;
        }

        public IEnumerable<IApplicationEvent> Events { get; init; }

        public void ClearEvents() => this.decorated.ClearEvents();

        public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var result = await this.decorated.Handle(request, cancellationToken);

            if (this.Events.Any())
            {
                await this.eventsDispatcherService.DispatchEventsAsync(new List<IEventsContainer<IApplicationEvent>> { this }, cancellationToken);
            }

            return result;
        }
    }
}
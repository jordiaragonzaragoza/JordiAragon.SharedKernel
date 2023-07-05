namespace JordiAragon.SharedKernel.Application.Commands.Decorators
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Application.Events.Services;

    public class ApplicationEventsDispatcherCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> decorated;
        private readonly IApplicationEventsDispatcherService applicationEventsDispatcherService;

        public ApplicationEventsDispatcherCommandHandlerDecorator(
            IApplicationEventsDispatcherService applicationEventsDispatcherService,
            ICommandHandler<TCommand> decorated)
        {
            this.applicationEventsDispatcherService = Guard.Against.Null(applicationEventsDispatcherService, nameof(applicationEventsDispatcherService));
            this.decorated = Guard.Against.Null(decorated, nameof(decorated));
            this.ApplicationEvents = decorated.ApplicationEvents;
        }

        public IEnumerable<IApplicationEvent> ApplicationEvents { get; init; }

        public async Task<Result> Handle(TCommand request, CancellationToken cancellationToken)
        {
            var result = await this.decorated.Handle(request, cancellationToken);

            if (this.ApplicationEvents.Any())
            {
                await this.applicationEventsDispatcherService.DispatchEventsAsync(this.ApplicationEvents, cancellationToken);
            }

            return result;
        }
    }
}
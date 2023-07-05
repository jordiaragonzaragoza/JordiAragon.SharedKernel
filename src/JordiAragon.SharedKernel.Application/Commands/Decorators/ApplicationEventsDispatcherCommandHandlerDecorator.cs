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

    public class ApplicationEventsDispatcherCommandHandlerDecorator<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        private readonly ICommandHandler<TCommand, TResponse> decorated;
        private readonly IApplicationEventsDispatcherService applicationEventsDispatcherService;

        public ApplicationEventsDispatcherCommandHandlerDecorator(
            IApplicationEventsDispatcherService applicationEventsDispatcherService,
            ICommandHandler<TCommand, TResponse> decorated)
        {
            this.applicationEventsDispatcherService = Guard.Against.Null(applicationEventsDispatcherService, nameof(applicationEventsDispatcherService));
            this.decorated = Guard.Against.Null(decorated, nameof(decorated));
            this.ApplicationEvents = decorated.ApplicationEvents;
        }

        public IEnumerable<IApplicationEvent> ApplicationEvents { get; init; }

        public async Task<Result<TResponse>> Handle(TCommand request, CancellationToken cancellationToken)
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
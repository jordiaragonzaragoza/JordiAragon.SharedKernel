namespace JordiAragon.SharedKernel.Application.Behaviours
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using MediatR;

    public class DomainEventsDispatcherBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse> // TODO: Review restrict to ICommand<TResponse> or ICommand
        where TResponse : IResult
    {
        private readonly IDomainEventsDispatcher domainEventsDispatcher;

        public DomainEventsDispatcherBehaviour(
            IDomainEventsDispatcher domainEventsDispatcher)
        {
            this.domainEventsDispatcher = domainEventsDispatcher;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            // Unique atomic transaction.
            // Dispatch Domain Events collection.
            // BEFORE committing data (EF SaveChanges) into the DB. This makes
            // a single transaction including side effects from the domain event
            // handlers that are using the same DbContext with Scope lifetime.
            await this.domainEventsDispatcher.DispatchDomainEventsAsync(cancellationToken);

            return response;
        }
    }
}

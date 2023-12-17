namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public abstract class BaseWriteStore : IWriteStore, IScopedDependency
    {
        private readonly BaseContext context;

        protected BaseWriteStore(BaseContext context)
        {
            this.context = Guard.Against.Null(context, nameof(context));
        }

        public IEnumerable<IEventsContainer<IEvent>> EventableEntities
            => this.context.ChangeTracker.Entries<IEventsContainer<IDomainEvent>>()
                            .Select(e => e.Entity)
                            .Where(entity => entity.Events.Any());
    }
}
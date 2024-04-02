namespace JordiAragon.SharedKernel.Infrastructure
{
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public class UnitOfWork : IUnitOfWork, IScopedDependency
    {
        private readonly IBusinessModelStore businessModelStore;
        private readonly IEventStore eventStore;

        public UnitOfWork(
            IBusinessModelStore businessModelStore,
            IEventStore eventStore)
        {
            this.businessModelStore = Guard.Against.Null(businessModelStore, nameof(businessModelStore));
            this.eventStore = Guard.Against.Null(eventStore, nameof(eventStore));
        }

        public virtual void BeginTransaction()
        {
            this.businessModelStore.BeginTransaction();
            this.eventStore.BeginTransaction();
        }

        public virtual async Task CommitTransactionAsync()
        {
            // TODO: Complete distributed transaction eventStore and businessModelStore.
            // Check: https://discuss.eventstore.com/t/event-store-and-transactions/401/11
            await this.eventStore.CommitTransactionAsync();
            await this.businessModelStore.CommitTransactionAsync();
        }

        public virtual void RollbackTransaction()
        {
            this.businessModelStore.RollbackTransaction();
            this.eventStore.RollbackTransaction();
        }
    }
}
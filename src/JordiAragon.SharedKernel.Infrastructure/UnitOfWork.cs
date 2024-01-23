namespace JordiAragon.SharedKernel.Infrastructure
{
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public class UnitOfWork : IUnitOfWork, IScopedDependency
    {
        private readonly IWriteStore writeStore;
        private readonly IEventStore eventStore;

        public UnitOfWork(
            IWriteStore writeStore,
            IEventStore eventStore)
        {
            this.writeStore = Guard.Against.Null(writeStore, nameof(writeStore));
            this.eventStore = Guard.Against.Null(eventStore, nameof(eventStore));
        }

        public virtual void BeginTransaction()
        {
            this.writeStore.BeginTransaction();
            this.eventStore.BeginTransaction();
        }

        public virtual async Task CommitTransactionAsync()
        {
            // TODO: Wrap on a System.Transaction
            // Check: https://discuss.eventstore.com/t/event-store-and-transactions/401/11
            await this.eventStore.CommitTransactionAsync();
            await this.writeStore.CommitTransactionAsync();
        }

        public virtual void RollbackTransaction()
        {
            this.writeStore.RollbackTransaction();
            this.eventStore.RollbackTransaction();
        }
    }
}
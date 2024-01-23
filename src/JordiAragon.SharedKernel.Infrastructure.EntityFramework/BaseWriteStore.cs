namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Contracts.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;
    using Microsoft.EntityFrameworkCore.Storage;

    public abstract class BaseWriteStore : IWriteStore, IDisposable, IScopedDependency
    {
        private readonly BaseContext context;
        private IDbContextTransaction transaction;

        protected BaseWriteStore(BaseContext context)
        {
            this.context = Guard.Against.Null(context, nameof(context));
        }

        public IEnumerable<IEventsContainer<IEvent>> EventableEntities
            => this.context.ChangeTracker.Entries<IEventsContainer<IDomainEvent>>()
                            .Select(e => e.Entity)
                            .Where(entity => entity.Events.Any());

        public void BeginTransaction()
        {
            if (this.transaction != null)
            {
                return;
            }

            this.transaction = this.context.Database.BeginTransaction();
        }

        public Task CommitTransactionAsync()
        {
            if (this.transaction == null)
            {
                return Task.CompletedTask;
            }

            this.transaction.Commit();
            this.transaction.Dispose();
            this.transaction = null;

            return Task.CompletedTask;
        }

        public void RollbackTransaction()
        {
            if (this.transaction == null)
            {
                return;
            }

            this.transaction.Rollback();
            this.transaction.Dispose();
            this.transaction = null;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.transaction == null)
            {
                return;
            }

            this.transaction.Dispose();
            this.transaction = null;
        }
    }
}
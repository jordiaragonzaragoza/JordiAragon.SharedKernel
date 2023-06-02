namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework
{
    using System;
    using System.Threading;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using Microsoft.EntityFrameworkCore.Storage;

    public abstract class BaseUnitOfWork : IUnitOfWork, IScopedDependency
    {
        private readonly BaseContext context;
        private IDbContextTransaction transaction;

        protected BaseUnitOfWork(
            BaseContext context)
        {
            this.context = context;
        }

        public virtual void BeginTransaction()
        {
            if (this.transaction != null)
            {
                return;
            }

            this.transaction = this.context.Database.BeginTransaction();
        }

        public virtual void CommitTransaction(CancellationToken cancellationToken = default)
        {
            if (this.transaction == null)
            {
                return;
            }

            this.transaction.Commit();
            this.transaction.Dispose();
            this.transaction = null;
        }

        public virtual void RollbackTransaction()
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
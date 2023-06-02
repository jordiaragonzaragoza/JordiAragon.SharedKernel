namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System;
    using System.Threading;

    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction(CancellationToken cancellationToken = default);

        void RollbackTransaction();
    }
}
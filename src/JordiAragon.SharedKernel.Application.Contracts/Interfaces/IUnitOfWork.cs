namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
namespace JordiAragon.SharedKernel.Application.Contracts.Interfaces
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        void BeginTransaction();

        Task CommitTransactionAsync();

        void RollbackTransaction();
    }
}
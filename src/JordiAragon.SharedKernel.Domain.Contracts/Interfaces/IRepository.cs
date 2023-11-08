namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using Ardalis.Specification;

    public interface IRepository<T, TId> : IRepositoryBase<T>
        where T : class, IAggregateRoot<TId>
    {
    }
}

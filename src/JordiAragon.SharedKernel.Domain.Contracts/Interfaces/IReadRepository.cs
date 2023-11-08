namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using Ardalis.Specification;

    public interface IReadRepository<T, TId> : IReadRepositoryBase<T>
        where T : class, IEntity<TId>
    {
    }
}
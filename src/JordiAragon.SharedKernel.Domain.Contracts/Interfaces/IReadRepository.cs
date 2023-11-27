namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using Ardalis.Specification;

    /// <summary>
    /// <para>
    /// A <see cref="IReadRepository{TEntity, TId, TIdType}" /> can be used to query instances of <typeparamref name="TEntity" />.
    /// An <see cref="ISpecification{TEntity}"/> (or derived) is used to encapsulate the LINQ queries against the database.
    /// </para>
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being operated on by this repository.</typeparam>
    /// <typeparam name="TId">The type of id entity being operated on by this repository.</typeparam>
    /// <typeparam name="TIdType">The id type of id entity being operated on by this repository.</typeparam>
    public interface IReadRepository<TEntity, TId, TIdType> : IReadRepositoryBase<TEntity>
        where TEntity : class, IEntity<TId, TIdType>
        where TId : IEntityId<TIdType>
    {
    }
}
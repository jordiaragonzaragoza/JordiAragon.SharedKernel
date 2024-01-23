namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// <para>
    /// A <see cref="IReadRepository{TEntity, TId}" /> can be used to query instances of <typeparamref name="TEntity" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being operated on by this repository.</typeparam>
    /// <typeparam name="TId">The type of id entity being operated on by this repository.</typeparam>
    public interface IReadRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : class, IEntityId
    {
        /// <summary>
        /// Finds an entity with the given primary key value.
        /// </summary>
        /// <param name="id">The value of the primary key for the entity to be found.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
        /// </returns>
        Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    }
}
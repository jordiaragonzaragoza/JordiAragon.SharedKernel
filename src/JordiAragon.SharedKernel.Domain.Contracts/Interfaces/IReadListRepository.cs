namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// <para>
    /// A <see cref="IReadListRepository{TEntity, TId}" /> can be used to query instances of <typeparamref name="TEntity" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being operated on by this repository.</typeparam>
    /// <typeparam name="TId">The type of id entity being operated on by this repository.</typeparam>
    public interface IReadListRepository<TEntity, TId> : IReadRepository<TEntity, TId>
        where TEntity : class, IEntity<TId>
        where TId : class, IEntityId
    {
        /// <summary>
        /// Finds all entities of <typeparamref name="TEntity" /> from the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a boolean whether any entity exists or not.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains true if the
        /// source sequence contains any elements; otherwise, false.
        /// </returns>
        Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    }
}
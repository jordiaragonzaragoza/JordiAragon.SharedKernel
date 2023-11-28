namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Specification;

    /// <summary>
    /// <para>
    /// A <see cref="ISpecificationReadRepository{TEntity, TId, TIdType}" /> can be used to query instances of <typeparamref name="TEntity" />.
    /// An <see cref="ISpecification{TEntity}"/> (or derived) is used to encapsulate the LINQ queries against the database.
    /// </para>
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being operated on by this repository.</typeparam>
    /// <typeparam name="TId">The type of id entity being operated on by this repository.</typeparam>
    /// <typeparam name="TIdType">The id type of id entity being operated on by this repository.</typeparam>
    public interface ISpecificationReadRepository<TEntity, TId, TIdType> : IReadRepository<TEntity, TId, TIdType>
        where TEntity : class, IEntity<TId, TIdType>
        where TId : IEntityId<TIdType>
    {
        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
        /// </returns>
        Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the first element of a sequence, or a default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="TResult" />, or <see langword="null"/>.
        /// </returns>
        Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="TEntity" />, or <see langword="null"/>.
        /// </returns>
        Task<TEntity> SingleOrDefaultAsync(ISingleResultSpecification<TEntity> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the only element of a sequence, or a default value if the sequence is empty; this method throws an exception if there is more than one element in the sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="TResult" />, or <see langword="null"/>.
        /// </returns>
        Task<TResult> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<List<TEntity>> ListAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// <para>
        /// Projects each entity into a new form, being <typeparamref name="TResult" />.
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{TResult}" /> that contains elements from the input sequence.
        /// </returns>
        Task<List<TResult>> ListAsync<TResult>(ISpecification<TEntity, TResult> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a number that represents how many entities satisfy the encapsulated query logic
        /// of the <paramref name="specification"/>.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the
        /// number of elements in the input sequence.
        /// </returns>
        Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the total number of records.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the
        /// number of elements in the input sequence.
        /// </returns>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a boolean that represents whether any entity satisfy the encapsulated query logic
        /// of the <paramref name="specification"/> or not.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains true if the
        /// source sequence contains any elements; otherwise, false.
        /// </returns>
        Task<bool> AnyAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds all entities of <typeparamref name="TEntity" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        ///  Returns an IAsyncEnumerable which can be enumerated asynchronously.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Ok to use Ardalis.Specification.IReadRepositoryBase")]
        IAsyncEnumerable<TEntity> AsAsyncEnumerable(ISpecification<TEntity> specification);
    }
}
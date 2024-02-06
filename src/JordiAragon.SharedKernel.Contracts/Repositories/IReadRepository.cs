namespace JordiAragon.SharedKernel.Contracts.Repositories
{
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Contracts.Model;

    /// <summary>
    /// <para>
    /// A <see cref="IReadRepository{TModel, TId}" /> can be used to query instances of <typeparamref name="TModel" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TModel">The type of model being operated on by this repository.</typeparam>
    /// <typeparam name="TId">The type of id model being operated on by this repository.</typeparam>
    public interface IReadRepository<TModel, TId>
        where TModel : class, IBaseModel<TId>
        where TId : notnull
    {
        /// <summary>
        /// Finds an model with the given primary key value.
        /// </summary>
        /// <param name="id">The value of the primary key for the model to be found.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="TModel" />, or <see langword="null"/>.
        /// </returns>
        Task<TModel> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    }
}
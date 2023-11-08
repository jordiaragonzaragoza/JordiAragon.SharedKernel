namespace JordiAragon.SharedKernel.Infrastructure.EventStore
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Specification;
    using global::EventStore.ClientAPI;
    using JordiAragon.SharedKernel.Contracts.DependencyInjection;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseReadRepository<T, TId> : IReadRepository<T, TId>, IScopedDependency
        where T : class, JordiAragon.SharedKernel.Domain.Contracts.Interfaces.IEntity<TId>
    {
        protected BaseReadRepository(IEventStoreConnection connection)
            => this.Connection = connection;

        protected IEventStoreConnection Connection { get; private init; }

        public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> GetByIdAsync<TIdx>(TIdx id, CancellationToken cancellationToken = default)
            where TIdx : notnull
        {
            throw new System.NotImplementedException();
        }

        public Task<T> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<TResult> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}

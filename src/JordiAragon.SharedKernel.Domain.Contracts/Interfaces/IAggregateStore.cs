namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Threading.Tasks;

    public interface IAggregateStore
    {
        Task<bool> ExistsAsync<T, TId>(TId aggregateId);

        Task SaveAsync<T, TId>(T aggregate)
            where T : IAggregateRoot<TId>;

        Task<T> LoadAsync<T, TId>(TId aggregateId)
            where T : IAggregateRoot<TId>;
    }
}
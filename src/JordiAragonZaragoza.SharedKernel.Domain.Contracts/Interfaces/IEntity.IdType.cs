namespace JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces
{
    /// <summary>
    /// Generic abstraction for a domain entity.
    /// </summary>
    /// <typeparam name="TId">The id for the entity.</typeparam>
    /// <typeparam name="TIdType">The id value for the entity.</typeparam>
    public interface IEntity<out TId, TIdType> : IEntity<TId> // TODO: Not in use. Remove ??
        where TId : class, IEntityId<TIdType>
        where TIdType : notnull
    {
    }
}
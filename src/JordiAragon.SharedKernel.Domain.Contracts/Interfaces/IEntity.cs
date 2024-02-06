namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using JordiAragon.SharedKernel.Contracts.Model;

    /// <summary>
    /// Generic abstraction for a domain entity.
    /// </summary>
    /// <typeparam name="TId">The id for the entity.</typeparam>
    public interface IEntity<out TId> : IBaseModel<TId>, IEntity
        where TId : class, IEntityId
    {
    }
}
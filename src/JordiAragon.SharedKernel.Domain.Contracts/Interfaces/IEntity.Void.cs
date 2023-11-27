namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    /// <summary>
    /// Generic abstraction for a domain entity.
    /// </summary>
    /// <typeparam name="TId">The id for the entity.</typeparam>
    public interface IEntity<out TId>
    {
        TId Id { get; }
    }
}
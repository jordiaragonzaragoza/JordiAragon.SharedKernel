namespace JordiAragonZaragoza.SharedKernel.Domain.Entities
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class EntityByIdSpec<TEntity, TId> : SingleResultSpecification<TEntity>
        where TEntity : class, JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces.IEntity<TId>
        where TId : class, IEntityId
    {
        public EntityByIdSpec(TId entityId)
        {
            Guard.Against.Null(entityId);

            this.Query
                .Where(entity => entity.Id == entityId);
        }
    }
}
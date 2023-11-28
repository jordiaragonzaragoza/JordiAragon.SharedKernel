namespace JordiAragon.SharedKernel.Domain.Entities
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;

    public sealed class EntityByIdSpec<TEntity, TId> : SingleResultSpecification<TEntity>
        where TEntity : class, JordiAragon.SharedKernel.Domain.Contracts.Interfaces.IEntity<TId>
        where TId : class
    {
        public EntityByIdSpec(TId entityId)
        {
            Guard.Against.Null(entityId);

            this.Query
                .Where(entity => entity.Id == entityId);
        }
    }
}
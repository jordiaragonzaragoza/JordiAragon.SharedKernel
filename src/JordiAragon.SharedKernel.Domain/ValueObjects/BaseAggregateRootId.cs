namespace JordiAragon.SharedKernel.Domain.ValueObjects
{
    public abstract class BaseAggregateRootId<TId> : BaseEntityId<TId>
        where TId : notnull
    {
        protected BaseAggregateRootId(TId value)
            : base(value)
        {
        }
    }
}
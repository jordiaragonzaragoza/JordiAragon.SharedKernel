namespace JordiAragon.SharedKernel.Domain.ValueObjects
{
    public abstract class BaseAggregateRootId<TId> : BaseEntityId<TId>
    {
        protected BaseAggregateRootId(TId value)
            : base(value)
        {
        }
    }
}
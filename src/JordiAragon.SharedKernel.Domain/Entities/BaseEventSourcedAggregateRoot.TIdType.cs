namespace JordiAragon.SharedKernel.Domain.Entities
{
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public abstract class BaseEventSourcedAggregateRoot<TId, TIdType> : BaseEventSourcedAggregateRoot<TId>
        where TId : BaseAggregateRootId<TIdType>
    {
        protected BaseEventSourcedAggregateRoot(TId id)
            : base(id)
        {
        }

        protected BaseEventSourcedAggregateRoot()
        {
        }
    }
}
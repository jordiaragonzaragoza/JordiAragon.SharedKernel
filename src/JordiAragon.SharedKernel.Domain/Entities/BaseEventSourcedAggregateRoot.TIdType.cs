namespace JordiAragon.SharedKernel.Domain.Entities
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public abstract class BaseEventSourcedAggregateRoot<TId, TIdType> : BaseEventSourcedAggregateRoot<TId>
        where TId : BaseAggregateRootId<TIdType>
    {
        protected BaseEventSourcedAggregateRoot(TId id)
            : base(id)
        {
            this.Id = Guard.Against.Null(id, nameof(id));
        }

        protected BaseEventSourcedAggregateRoot()
        {
        }

        // TODO: Check. Probably not required on event sourced aggregates. Remove this workaround when EF supports ValueObjects collections.
        // https://github.com/dotnet/efcore/issues/31237
        public new BaseAggregateRootId<TIdType> Id { get; protected set; }
    }
}
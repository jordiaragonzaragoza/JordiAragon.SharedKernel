namespace JordiAragon.SharedKernel.Domain.Entities
{
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public abstract class BaseAggregateRoot<TId, TIdType> : BaseAggregateRoot<TId>
        where TId : BaseAggregateRootId<TIdType>
        where TIdType : notnull
    {
        protected BaseAggregateRoot(TId id)
            : base(id)
        {
            this.Id = Guard.Against.Null(id, nameof(id));
        }

        // Required by EF.
        protected BaseAggregateRoot()
        {
        }

        // TODO: Remove this workaround when EF supports ValueObjects collections.
        // https://github.com/dotnet/efcore/issues/31237
        public new BaseAggregateRootId<TIdType> Id { get; protected set; } = default!;
    }
}
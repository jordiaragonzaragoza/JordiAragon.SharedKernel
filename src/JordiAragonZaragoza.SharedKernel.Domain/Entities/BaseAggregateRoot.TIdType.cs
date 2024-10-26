namespace JordiAragonZaragoza.SharedKernel.Domain.Entities
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1061:Do not hide base class methods", Justification = "Workaround required by EF on ValueObjects collections")]
        public new BaseAggregateRootId<TIdType> Id { get; protected set; } = default!;
    }
}
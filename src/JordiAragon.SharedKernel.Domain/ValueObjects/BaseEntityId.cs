namespace JordiAragon.SharedKernel.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public abstract class BaseEntityId<TIdType> : BaseValueObject, IEntityId<TIdType>
    {
        protected BaseEntityId(TIdType value)
        {
            Guard.Against.Default(value, nameof(value));
            Guard.Against.Null(value, nameof(value));

            this.Value = value;
        }

        // Required by EF.
        protected BaseEntityId()
        {
        }

        public TIdType Value { get; init; }

        public static implicit operator TIdType(BaseEntityId<TIdType> self)
        {
            Guard.Against.Null(self, nameof(self));

            return self.Value;
        }

        public override string ToString() => this.Value?.ToString() ?? base.ToString();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}
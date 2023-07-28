namespace JordiAragon.SharedKernel.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Ardalis.GuardClauses;

    public abstract class BaseEntityId<TId> : BaseValueObject
    {
        protected BaseEntityId(TId value)
        {
            this.Value = Guard.Against.Null(value, nameof(value));
        }

        // Required by EF.
        protected BaseEntityId()
        {
        }

        public TId Value { get; init; }

        public static implicit operator TId(BaseEntityId<TId> self)
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
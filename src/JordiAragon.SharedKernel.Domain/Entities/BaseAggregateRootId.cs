namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public abstract class BaseAggregateRootId<TId> : BaseValueObject
    {
        public abstract TId Value { get; protected set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}
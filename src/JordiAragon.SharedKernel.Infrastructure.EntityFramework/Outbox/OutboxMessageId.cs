namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class OutboxMessageId : BaseValueObject
    {
        private OutboxMessageId(Guid value)
        {
            this.Value = value;
        }

        public Guid Value { get; init; }

        public static OutboxMessageId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new OutboxMessageId(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}
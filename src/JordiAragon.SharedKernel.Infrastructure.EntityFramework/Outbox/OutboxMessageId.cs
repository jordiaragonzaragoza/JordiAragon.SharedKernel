namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class OutboxMessageId : BaseAggregateRootId<Guid>
    {
        private OutboxMessageId(Guid value)
            : base(value)
        {
        }

        public static OutboxMessageId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new OutboxMessageId(id);
        }
    }
}
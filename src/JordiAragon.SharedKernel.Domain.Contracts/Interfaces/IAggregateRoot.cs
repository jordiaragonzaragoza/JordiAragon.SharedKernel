﻿namespace JordiAragon.SharedKernel.Domain.Contracts.Interfaces
{
    using System.Collections.Generic;

    // Apply this marker interface only to aggregate root entities
    // Write repositories will only work with aggregate roots, not their children
    public interface IAggregateRoot<out TId> : IEntity<TId>
    {
        bool IsDeleted { get; }

        IEnumerable<IDomainEvent> DomainEvents { get; }
    }
}
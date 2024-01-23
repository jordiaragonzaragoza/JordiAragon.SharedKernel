namespace JordiAragon.SharedKernel.Infrastructure.Interfaces
{
    using System;

    public interface IAuditableEntity
    {
        DateTimeOffset CreationDateOnUtc { get; }

        string CreatedByUserId { get; }

        DateTimeOffset ModificationDateOnUtc { get; }

        string LastModifiedByUserId { get; }
    }
}
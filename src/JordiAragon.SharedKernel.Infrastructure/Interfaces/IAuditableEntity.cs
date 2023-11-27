namespace JordiAragon.SharedKernel.Infrastructure.Interfaces
{
    using System;

    public interface IAuditableEntity
    {
        DateTime CreationDateOnUtc { get; }

        string CreatedByUserId { get; }

        DateTime ModificationDateOnUtc { get; }

        string LastModifiedByUserId { get; }
    }
}
namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Audit
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public abstract class BaseAuditableDataEntity : IAuditableEntity
    {
        public DateTimeOffset CreationDateOnUtc { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTimeOffset ModificationDateOnUtc { get; set; }

        public string LastModifiedByUserId { get; set; }
    }
}

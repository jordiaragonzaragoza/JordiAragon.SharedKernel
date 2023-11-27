namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Audit
{
    using System;
    using JordiAragon.SharedKernel.Infrastructure.Interfaces;

    public abstract class BaseAuditableDataEntity : IAuditableEntity
    {
        public DateTime CreationDateOnUtc { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTime ModificationDateOnUtc { get; set; }

        public string LastModifiedByUserId { get; set; }

        // TODO: Implement SoftDelete
        ////public DateTime? DeletionDate { get; set; }
        ////public string DeletedByUserId { get; set; }
    }
}

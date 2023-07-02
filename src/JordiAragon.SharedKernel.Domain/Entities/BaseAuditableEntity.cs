namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System;

    // TODO: Move to an interface for required entities
    public abstract class BaseAuditableEntity<TId> : BaseEntity<TId>
    {
        protected BaseAuditableEntity(TId id)
            : base(id)
        {
        }

        // Required by EF.
        protected BaseAuditableEntity()
        {
        }

        public DateTime CreationDateOnUtc { get; set; }

        public string CreatedByUserId { get; set; }

        public DateTime ModificationDateOnUtc { get; set; }

        public string LastModifiedByUserId { get; set; }

        ////public DateTime? DeletionDate { get; set; }
        ////public string DeletedByUserId { get; set; }
    }
}

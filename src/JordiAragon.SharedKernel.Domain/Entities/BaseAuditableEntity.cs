namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System;

    // TODO: Move to an interface and add soft delete for required entities
    public abstract class BaseAuditableEntity<TId> : BaseEventableEntity<TId>
    {
        protected BaseAuditableEntity(TId id)
            : base(id)
        {
        }

        protected BaseAuditableEntity()
            : base()
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

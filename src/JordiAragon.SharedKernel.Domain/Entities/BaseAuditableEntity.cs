namespace JordiAragon.SharedKernel.Domain.Entities
{
    using System;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    // TODO: Move this base class to a data entity. It is not part of the domain
    public abstract class BaseAuditableEntity<TId> : BaseEntity<TId>, IAuditableEntity
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

        // TODO: Implement SoftDelete
        ////public DateTime? DeletionDate { get; set; }
        ////public string DeletedByUserId { get; set; }
    }
}

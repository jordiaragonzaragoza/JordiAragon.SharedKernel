namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Interceptors
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Audit;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Helpers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    /// <summary>
    /// Its conserved as future reference. Audit is not yet implemented.
    /// </summary>
    public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTimeService;

        public AuditableEntitySaveChangesInterceptor(
            ICurrentUserService currentUserService,
            IDateTime dateTimeService)
        {
            this.currentUserService = currentUserService;
            this.dateTimeService = dateTimeService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            this.UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            this.UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext context)
        {
            if (context == null)
            {
                return;
            }

            foreach (var entry in context.ChangeTracker.Entries())
            {
                Type entityType = entry.Entity.GetType();

                var idProperty = Array.Find(entityType.GetProperties(), p => p.Name == "Id");
                if (idProperty != null)
                {
                    Type baseAuditableEntityType = typeof(BaseAuditableDataEntity);

                    ////Type[] typeArgs = { idProperty.PropertyType };
                    ////Type auditableEntityType = baseAuditableEntityType.MakeGenericType(typeArgs);

                    if (baseAuditableEntityType.IsAssignableFrom(entityType))
                    {
                        dynamic auditableEntity = entry.Entity;

                        if (entry.State == EntityState.Added)
                        {
                            auditableEntity.CreatedByUserId = this.currentUserService.UserId;
                            auditableEntity.CreationDateOnUtc = this.dateTimeService.UtcNow;
                        }

                        if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
                        {
                            auditableEntity.LastModifiedByUserId = this.currentUserService.UserId;
                            auditableEntity.ModificationDateOnUtc = this.dateTimeService.UtcNow;
                        }
                    }
                }
            }
        }
    }
}
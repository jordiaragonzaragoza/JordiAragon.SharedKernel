namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Interceptors
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Helpers;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

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
                Type baseAuditableEntityType = typeof(BaseAuditableEntity<>);

                Type entityType = entry.Entity.GetType();

                var idProperty = entityType.GetProperties().FirstOrDefault(p => p.Name == "Id");
                if (idProperty != null)
                {
                    Type[] typeArgs = { idProperty.PropertyType };

                    Type auditableEntityType = baseAuditableEntityType.MakeGenericType(typeArgs);

                    if (auditableEntityType.IsAssignableFrom(entityType))
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
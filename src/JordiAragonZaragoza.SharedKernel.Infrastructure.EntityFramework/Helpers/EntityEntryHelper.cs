namespace JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Helpers
{
    using System.Linq;
    using Ardalis.GuardClauses;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public static class EntityEntryHelper
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry)
        {
            Guard.Against.Null(entry, nameof(entry));

            return entry.References.Any(referenceEntry =>
                referenceEntry.TargetEntry != null &&
                referenceEntry.TargetEntry.Metadata.IsOwned() &&
                (referenceEntry.TargetEntry.State == EntityState.Added || referenceEntry.TargetEntry.State == EntityState.Modified));
        }
    }
}
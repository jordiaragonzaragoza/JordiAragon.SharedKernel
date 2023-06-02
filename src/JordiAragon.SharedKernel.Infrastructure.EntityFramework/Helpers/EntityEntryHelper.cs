namespace JordiAragon.SharedKernel.Infrastructure.EntityFramework.Helpers
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    public static class EntityEntryHelper
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(referenceEntry =>
                referenceEntry.TargetEntry != null &&
                referenceEntry.TargetEntry.Metadata.IsOwned() &&
                (referenceEntry.TargetEntry.State == EntityState.Added || referenceEntry.TargetEntry.State == EntityState.Modified));
    }
}
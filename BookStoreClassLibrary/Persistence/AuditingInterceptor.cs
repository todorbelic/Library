using BookStoreClassLibrary.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookStoreClassLibrary.Persistence
{
    public class AuditingInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;
            if (dbContext == null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            IEnumerable<EntityEntry<IAuditable>> entries =
                dbContext
                    .ChangeTracker
                    .Entries<IAuditable>();

            foreach (var entry in entries)
            {
                DateTime timeStamp = DateTime.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entry.Property(a => a.CreatedDate).CurrentValue = timeStamp;
                    entry.Property(a => a.ModifiedDate).CurrentValue = timeStamp;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(a => a.ModifiedDate).CurrentValue = timeStamp;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}

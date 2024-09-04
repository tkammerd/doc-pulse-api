using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities._Kernel;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class AuditBaseDbContextExtensions
{
    public static async Task UpdateAllShadowColumns(this DbContext dbContext, UserWithClaims _user)
    {
        await dbContext.UpdateSqlGenShadowColumns();
        await dbContext.UpdateAuditableColumns(_user);
    }

    public static async Task<int> UpdateSqlGenShadowColumns(this DbContext dbContext)
    {
        int num = 0;
        foreach (EntityEntry<ISqlGenTimestampBase> item in (dbContext.ChangeTracker.Entries<ISqlGenTimestampBase>() ?? Enumerable.Empty<EntityEntry<ISqlGenTimestampBase>>()).Where((o) => o.State == EntityState.Modified))
        {
            item.Property("SqlModified").CurrentValue = DateTimeOffset.Now;
            num++;
        }

        return await Task.FromResult(num);
    }

    public static async Task UpdateAuditableColumns(this DbContext dbContext, UserWithClaims _user)
    {
        if (_user.IsNullUser) return;

        IEnumerable<EntityEntry<IAuditableEntityBase>> auditable = dbContext.ChangeTracker.Entries<IAuditableEntityBase>() ?? Enumerable.Empty<EntityEntry<IAuditableEntityBase>>();
        if (!auditable.Where((e) => e.State == EntityState.Added || e.State == EntityState.Modified).Any())
        {
            return;
        }

        int dbUserIdLookup = 0;
        UserStub? userStub = null;

        try
        {
            DbSet<UserStub> dbSet = dbContext.Set<UserStub>();
            userStub = await dbSet.FirstOrDefaultAsync((o) => o.Identifier == _user.Identifier)
                        ?? (await dbSet.AddAsync(new UserStub
                        {
                            Identifier = _user.Identifier,
                            DisplayName = _user.DisplayName
                        })).Entity;

            if (!userStub.DisplayName.Equals(_user.DisplayName) && !_user.DisplayName.Equals("Unknown"))
            {
                userStub.DisplayName = _user.DisplayName;
            }

            if (userStub.Id != 0)
            {
                dbUserIdLookup = userStub.Id;
                userStub = null;
            }
        }
        catch (Exception)
        {
        }

        foreach (EntityEntry<IAuditableEntityBase> item in auditable)
        {
            item.Reference((e) => e.CreatedUser).IsModified = false;
            item.Property((e) => e.Created).IsModified = false;
            item.Property((e) => e.CreatedUserId).IsModified = false;
            item.Property((e) => e.Modified).IsModified = false;
            item.Property((e) => e.ModifiedUserId).IsModified = false;
            item.Reference((e) => e.ModifiedUser).IsModified = false;
        }

        foreach (EntityEntry<IAuditableEntityBase> item2 in auditable.Where((o) => o.State == EntityState.Added))
        {
            item2.Entity.Created = DateTimeOffset.Now;
            if (dbUserIdLookup == 0)
                item2.Entity.CreatedUser = userStub;
            else
                item2.Entity.CreatedUserId = dbUserIdLookup;

            item2.Entity.Modified = DateTimeOffset.Now;
            if (dbUserIdLookup == 0)
                item2.Entity.ModifiedUser = userStub;
            else
                item2.Entity.ModifiedUserId = dbUserIdLookup;
        }

        foreach (EntityEntry<IAuditableEntityBase> item3 in auditable.Where((o) => o.State == EntityState.Modified))
        {
            if (dbContext.ChangeTracker.HasChanges())
            {
                item3.Entity.Modified = DateTime.Now;
                if (dbUserIdLookup == 0)
                    item3.Entity.ModifiedUser = userStub;
                else
                    item3.Entity.ModifiedUserId = dbUserIdLookup;
            }
        }
    }
}


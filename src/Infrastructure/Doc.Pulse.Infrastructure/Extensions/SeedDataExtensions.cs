using Microsoft.EntityFrameworkCore;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class SeedDataExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Security", "EF1002:Risk of vulnerability to SQL injection.",
        Justification = @"
                         Suppress EF1002 for ExecuteSqlRaw.  ExecuteSql method fails due to some
                         parameterization issue, and since the value of tableName does not come
                         from the user, it is not vulnerable to SQL injection.
                        "
    )]
    public static void SeedRangeWithIdentityInsertOn<T>(this DbContext dbContext, List<T> entities) where T : class
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        if (dbContext.Database.IsInMemory())
        {
            dbContext.Set<T>().AddRange(entities);

            dbContext.SaveChanges();
        }
        else
        {
            var entityType = dbContext.Model.GetEntityTypes().First(t => t.ClrType == typeof(T));
            var tableName = entityType.GetAnnotation("Relational:TableName").Value?.ToString();
            var schemaName = entityType.GetAnnotation("Relational:Schema").Value?.ToString();

            ArgumentNullException.ThrowIfNull(tableName);
            ArgumentNullException.ThrowIfNull(schemaName);

            using var transaction = dbContext.Database.BeginTransaction();
            dbContext.Set<T>().AddRange(entities);
            dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {schemaName}.{tableName} ON;");
            dbContext.SaveChanges();
            dbContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {schemaName}.{tableName} OFF;");
            transaction.Commit();
        }
    }

    public static void SeedRangeWithIdentityInsertOff<T>(this DbContext dbContext, List<T> entities) where T : class
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        if (dbContext.Database.IsInMemory())
        {
            dbContext.Set<T>().AddRange(entities);

            dbContext.SaveChanges();
        }
        else
        {
            var entityType = dbContext.Model.GetEntityTypes().First(t => t.ClrType == typeof(T));
            var tableName = entityType.GetAnnotation("Relational:TableName").Value?.ToString();
            var schemaName = entityType.GetAnnotation("Relational:Schema").Value?.ToString();

            ArgumentNullException.ThrowIfNull(tableName);
            ArgumentNullException.ThrowIfNull(schemaName);

            using var transaction = dbContext.Database.BeginTransaction();
            dbContext.Set<T>().AddRange(entities);
            dbContext.SaveChanges();
            transaction.Commit();
        }
    }

}

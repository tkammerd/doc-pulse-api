using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Doc.Pulse.DatabaseLoader;
internal static class ServiceProviderExtensions
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Security", "EF1002:Risk of vulnerability to SQL injection.",
        Justification = @"
                         Suppress EF1002 for ExecuteSqlRaw.  ExecuteSql method fails due to some
                         parameterization issue, and since the value of tableName does not come
                         from the user, it is not vulnerable to SQL injection.
                        "
    )]
    public static ServiceProvider WipeoutDatabase(this ServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<AppDbContext>();

            dbContext.Database.EnsureCreated();

            var tables = dbContext.Model.GetEntityTypes()
                .Select(o => new { 
                    Schema = Helpers.CoalesceWithEmptyString(o.GetSchema(), o.GetDefaultSchema(), "dto"),
                    Table = o.GetTableName() })
                .Distinct().ToList();
            foreach (var table in tables)
            {
                dbContext.Database.ExecuteSqlRaw(
                    $@"IF EXISTS(SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = '{table.Table}' AND last_value IS NULL)
                          DBCC CHECKIDENT ('[{table.Schema}].[{table.Table}]', RESEED, 1)
                      ELSE IF EXISTS(SELECT * from sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = '{table.Table}')
                          DBCC CHECKIDENT ('[{table.Schema}].[{table.Table}]', RESEED, 0)"
                );
            }
            Console.WriteLine($"Reset All Identities!");

            dbContext.ObjectCodes.Purge();
            dbContext.CodeCategories.Purge();
            dbContext.UserStubs.Purge();

            dbContext.SaveChanges();
        }
        return serviceProvider;
    }

    public static ServiceProvider SeedData<T>(this ServiceProvider serviceProvider, IEnumerable<T> entities) where T : class
    {
        Console.WriteLine($"Seed {(typeof(T)?.Name ?? "Unknown")}(s)");

        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var dbContext = scopedServices.GetRequiredService<AppDbContext>();

            dbContext.Database.EnsureCreated();
            //SeedData.PopulateTestData(db);

            dbContext.SeedRangeWithIdentityInsertOff(entities.ToList());
        }

        return serviceProvider;
    }


    private static void Purge<T>(this DbSet<T> dbSet) where T : class
    {
        dbSet.RemoveRange(dbSet);
        Console.WriteLine($"Entity [{typeof(T).Name}] Marked for Purge!");
    }

    //private static void PurgeEntity<T>(this DbContext dbContext, T? entity) where T : class
    //{
    //    if (entity == null) return;

    //    var dbSet = dbContext.Set<T>();
    //    dbSet.Purge();
    //}
}


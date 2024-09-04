using Doc.Pulse.Core.Entities.ApiInformationAggregate;
using Doc.Pulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Tests.Functional.TestSeedData;

public static partial class FunctionalTestSeedData
{
    //public static void Initialize(IServiceProvider serviceProvider)
    //{
    //    using (var dbContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), default!, default!))
    //    {
    //        PopulateTestData(dbContext);
    //    }
    //}

    //public static void PopulateTestData(AppDbContext dbContext)
    //{
    //    try
    //    {
    //        //dbContext.SeedRangeWithIdentityInsertOff(Statuses);
    //        //dbContext.SeedRangeWithIdentityInsertOn(Statuses);

    //        dbContext.SeedRangeWithIdentityInsertOff(Funds);


    //        // ----------------------------------------------------------------------------------
    //        // Replaced with 'SeedRangeWithIdentityInsert' Extension - Here for Documentation Purposes (In case extension does not work in all cases)
    //        //using (var transaction = dbContext.Database.BeginTransaction())
    //        //{
    //        //    dbContext.Sections2.AddRange(Sections);
    //        //    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT faqs.Sections ON;");
    //        //    dbContext.SaveChanges();
    //        //    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT faqs.Sections OFF");
    //        //    transaction.Commit();
    //        //}
    //        // ----------------------------------------------------------------------------------
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
}
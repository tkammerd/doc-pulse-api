using Doc.Pulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Doc.Pulse.Infrastructure.Extensions;

namespace Doc.Pulse.Api;

public static partial class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var dbContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>(), default!, default!))
        {
            PopulateTestData(dbContext);
        }
    }

    public static void PopulateTestData(AppDbContext dbContext)
    {
        try
        {
            //dbContext.SeedRangeWithIdentityInsertOff(Statuses);
            //dbContext.SeedRangeWithIdentityInsertOn(Statuses);

            // TODO - maybe a appsetting to trigger a wipe ???

            //if (!dbContext.ApiInformations.Any())
            //{
            //    dbContext.SeedRangeWithIdentityInsertOff(ApiInformation);
            //}
            //if (!dbContext.Samples.Any())
            //{
            //    dbContext.SeedRangeWithIdentityInsertOff(SimpleSamples);
            //}

            //if (!dbContext.Codes.Any())
            //{
            //    dbContext.SeedRangeWithIdentityInsertOn(CodesSeed);
            //}
            //if (!dbContext.IsisCodes.Any())
            //{
            //    dbContext.SeedRangeWithIdentityInsertOn(IsisCodesSeed);
            //}

            // ----------------------------------------------------------------------------------
            // Replaced with 'SeedRangeWithIdentityInsert' Extension - Here for Documentation Purposes (In case extension does not work in all cases)
            //using (var transaction = dbContext.Database.BeginTransaction())
            //{
            //    dbContext.Sections2.AddRange(Sections);
            //    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT faqs.Sections ON;");
            //    dbContext.SaveChanges();
            //    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT faqs.Sections OFF");
            //    transaction.Commit();
            //}
            // ----------------------------------------------------------------------------------
        }
        catch (Exception)
        {
            throw;
        }
    }
}
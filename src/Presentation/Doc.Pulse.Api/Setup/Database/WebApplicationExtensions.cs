using Doc.Pulse.Infrastructure.Data;

namespace Doc.Pulse.Api.Setup.Database;

public static class WebApplicationExtensions
{
    public static WebApplication SeedTestData(this WebApplication app)
    {
        var serviceProvider = app.Services;
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            db.Database.EnsureCreated();
            SeedData.PopulateTestData(db);
        }

        return app;
    }
}

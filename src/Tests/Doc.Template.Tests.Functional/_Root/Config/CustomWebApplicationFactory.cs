using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Config;
using Doc.Pulse.Core.Entities;
using Doc.Pulse.Infrastructure.Data;
using Doc.Pulse.Infrastructure.Extensions;
using Doc.Pulse.Tests.Functional.TestSeedData;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Doc.Pulse.Tests.Functional._Root.Config;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Program>
{
    /// <summary>
    /// Overriding CreateHost to avoid creating a separate ServiceProvider per this thread:
    /// https://github.com/dotnet-architecture/eShopOnWeb/issues/465
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    //protected override IHost CreateHost(IHostBuilder builder)
    //{
    //    var host = builder.Build();

    //    // Get service provider.
    //    var serviceProvider = host.Services;

    //    // Create a scope to obtain a reference to the database
    //    // context (AppDbContext).
    //    using (var scope = serviceProvider.CreateScope())
    //    {
    //        var scopedServices = scope.ServiceProvider;
    //        var dbContext = scopedServices.GetRequiredService<AppDbContext>();

    //        var logger = scopedServices
    //            .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

    //        // Ensure the database is created.
    //        dbContext.Database.EnsureCreated();

    //        try
    //        {
    //            // Seed the database with test data.
    //            FunctionalTestSeedData.PopulateTestData(dbContext);
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.LogError(ex, "An error occurred seeding the " +
    //                                $"database with test messages. Error: {ex.Message}");
    //        }
    //    }

    //    host.Start();
    //    return host;
    //}

    //protected override void ConfigureWebHost(IWebHostBuilder builder)
    //{
    //    builder
    //        .UseSolutionRelativeContentRoot(Globals.ApiContextRootSolution) 
    //        .ConfigureServices(services =>
    //        {
    //            // Remove the app's ApplicationDbContext registration.
    //            var descriptor = services.SingleOrDefault(
    //                d => d.ServiceType ==
    //                    typeof(DbContextOptions<AppDbContext>));

    //            if (descriptor != null)
    //            {
    //                services.Remove(descriptor);
    //            }

    //            // This should be set for each individual test run
    //            string inMemoryCollectionName = Guid.NewGuid().ToString();

    //            // Add ApplicationDbContext using an in-memory database for testing.
    //            services.AddDbContext<AppDbContext>(options =>
    //            {
    //                options.UseInMemoryDatabase(inMemoryCollectionName);
    //                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    //            });

    //            //services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
    //            //{
    //            //    var config = new OpenIdConnectConfiguration()
    //            //    {
    //            //        Issuer = MockJwtTokens.Issuer,
    //            //    };

    //            //    config.SigningKeys.Add(MockJwtTokens.SecurityKey);
    //            //    options.Configuration = config;
    //            //});

    //            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
    //            //services.AddMvc(opt => opt.Filters.Add(new AllowAnonymousFilter()));
    //            services.AddScoped<IResolveUserService, FakeResolveUserService>();

    //            //services.AddScoped<IMediator, NoOpMediator>();
    //        });
    //}
}
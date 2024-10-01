using Doc.Pulse.Api.Setup;
using Doc.Pulse.Api.Setup.Auth;
using Doc.Pulse.Api.Setup.Configuration;
using Doc.Pulse.Api.Setup.Logging;
using Doc.Pulse.Api.Setup.Swagger;
using Doc.Pulse.Infrastructure.Config;
using Doc.Pulse.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OtsLogger;
using Serilog;
using System.Reflection;


try
{
    Log.Logger = OtsLoggingHelpers.CreateLogger();

    var builder = AppDmDocWebApplication.CreateBuilder(args);
    var environment = builder.Environment;

    builder.Services.AddEsbEmailing(environment, builder.Configuration);

    builder.Services
        .ConfigureAllDocSharedKernelServices(builder.Configuration, typeof(Program).Assembly);
    //    .ConfigureAllDocSharedKernelServices(builder.Configuration, typeof(Program).Assembly, typeof(EmailService).Assembly, typeof(ICcClient).Assembly, typeof(Globals).Assembly);
    //    .ConfigureAllDocSharedKernelServices(builder.Configuration, AppDomain.CurrentDomain.GetAssemblies());

    builder.ConfigureKestrelToAllowSelfSignedDevCertForOtsAuth();

    string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
                        connectionString, x => x.MigrationsHistoryTable("__MyMigrationsHistory", "core") ));

    builder.Services
        .AddOtsAuthorization(environment, builder.Configuration)
        .AddOtsAuthentication(environment, builder.Configuration);


    builder.Services
        .AddControllers()
        .AddMvcOptions(options =>
        {
            var groupClaim = builder.Configuration["Auth:GroupClaim"] ?? throw new Exception("Required Configuration 'Auth:GroupClaim' Not Found");
            var allowedGroups = builder.Configuration.GetSection("Auth:AllowedGroups").Get<string[]>() ?? throw new Exception("Required Configuration 'Auth:AllowedGroups' Not Found");

            var policyBuilder = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireClaim(groupClaim, allowedGroups);

            var policy = policyBuilder.Build();

            options.Filters.Add(new AuthorizeFilter(policy));
        })
        .AddValidationFailedBehavior();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddMapper();
    builder.Services.EnableVersioning();
    builder.Services.AddSwaggerDocumentation("Pulse API");

    // //////////////////////////////////////////////////////////////////////////
    // Added from https://medium.com/@egwudaujenyuojo/implement-api-documentation-in-net-7-swagger-openapi-and-xml-comments-214caf53eece
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    //Swagger Documentation Section
    var info = new OpenApiInfo()
    {
        Title = "Pulse API Documentation",
        Version = "v1",
        Description = "API Endpoints for PULSE modernization",
        Contact = new OpenApiContact()
        {
            Name = "Troy Kammerdiener",
            Email = "troy.kammerdiener@la.gov",
        }

    };

    builder.Services.AddSwaggerGen(c =>
    {
        // Line below already added by  https://medium.com/@egwudaujenyuojo/implement-api-documentation-in-net-7-swagger-openapi-and-xml-comments-214caf53eece 
        //c.SwaggerDoc("v1", info);

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
    // //////////////////////////////////////////////////////////////////////////

    if (builder.Configuration.IsAuthDisabledAndNotStagingOrProduction(environment))
    {
        builder.Services.AddSingleton<IPolicyEvaluator, AlwaysSuccessPolicyEvaluator>();
    }
    

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsProduction())
    {
        app.UseSwaggerDocumentation();

        // //////////////////////////////////////////////////////////////////////////
        // Added from https://medium.com/@egwudaujenyuojo/implement-api-documentation-in-net-7-swagger-openapi-and-xml-comments-214caf53eece
        app.UseSwagger(u =>
        {
            u.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "swagger";
            c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Your API Title or Version");
        });
        // //////////////////////////////////////////////////////////////////////////
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseOtsLogger();

    //app.UseOtsConfirmationValidation();

    app.DebugAuthSetBreakpointInHere();
    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Starting BuildWebHost ...");
    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("HostAbortedException", StringComparison.Ordinal) ||
        type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, "Application start-up failed");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
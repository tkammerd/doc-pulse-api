namespace Doc.Pulse.Api.Setup;

public static class AppDmDocWebApplication
{

    public static WebApplicationBuilder CreateBuilder(params string[] args)
    {
        var _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var builder = (_env != "dev"
            ? WebApplication.CreateBuilder(args)
            : WebApplication.CreateBuilder(new WebApplicationOptions
            {
                EnvironmentName = Environments.Development, /// Need to Trust Self Signed Certificates
                Args = args
            }));

        _ = builder.Configuration
                .AddJsonFile($"appsettings-logging.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings-logging.{_env}.json", optional: true, reloadOnChange: true);

        if (_env == "dev")
        {
            builder.Configuration.Sources.Clear();

            _ = builder.Configuration
                .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_env}.json", optional: true, reloadOnChange: true);
        }


        return builder;
    }
}

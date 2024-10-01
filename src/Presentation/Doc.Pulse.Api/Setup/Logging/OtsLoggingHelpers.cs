using OtsLogger;
using Serilog;

namespace Doc.Pulse.Api.Setup.Logging;

public static class OtsLoggingHelpers
{
    public static Serilog.Core.Logger CreateLogger()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile($"appsettings-logging.json", optional: true, reloadOnChange: true)
             .AddJsonFile($"appsettings-logging.{env}.json", optional: true, reloadOnChange: true)
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env}.json", optional: true)
             .Build();

        var logger = new LoggerConfiguration()
              .AddOtsConfiguration(configuration)
              .CreateLogger();

        logger.Information("Setup OTS Logger ...");

        return logger;
    }

}


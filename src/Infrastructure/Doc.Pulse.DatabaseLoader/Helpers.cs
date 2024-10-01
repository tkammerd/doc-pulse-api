using CsvHelper;
using Doc.Pulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Doc.Pulse.DatabaseLoader;
internal class Helpers
{
    public static ServiceProvider Setup()
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("settings/appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"settings/appsettings.{environmentName}.json", optional: true)
            .Build();

        string? connectionString = config.GetConnectionString("DefaultConnection");
       
        var serviceProviderBuilder = new ServiceCollection()
            .AddLogging()
            .AddScoped<IConfiguration>(_ => config)
            .AddDbContext<AppDbContext>(opt => opt.UseSqlServer(
                            connectionString, x => x.MigrationsHistoryTable("__MyMigrationsHistory", "core")));
        ;

        return serviceProviderBuilder.BuildServiceProvider();
    }
    public static List<T> ParseRecords<T>(string filePath)
    {
        var records = new List<T>();

        using (var reader = new StreamReader(filePath, new FileStreamOptions() { Access = FileAccess.Read, Mode = FileMode.Open, Share = FileShare.Read }))
        {
            var csvConfig = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true, 
                HeaderValidated = null,
                MissingFieldFound = null, 
                TrimOptions = CsvHelper.Configuration.TrimOptions.Trim
            };

            using var csv = new CsvReader(reader, csvConfig);
            var fileRecords = csv.GetRecords<T>();
            records.AddRange(fileRecords);
        }

        return records;
    }

    //public static string? CoalesceWithEmptyString(string? value1, string? value2)
    public static string? CoalesceWithEmptyString(params string?[] values)
    {
        if (values == null || values.Length == 0) return null;

        if (string.IsNullOrEmpty(values[0]))
            return CoalesceWithEmptyString(values[1..]);

        return values[0];
    }
}

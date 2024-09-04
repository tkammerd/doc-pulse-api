namespace Doc.Pulse.Api.Setup.Auth;

public static class IConfigurationExtensions
{
    public static bool UseCertificateForwarding(this IConfiguration configuration)
    {
        var useCertForwarding = configuration["Auth:UseCertificateForwarding"] ?? "false";

        return useCertForwarding.Equals("true", StringComparison.CurrentCultureIgnoreCase);
    }

    public static bool IsAuthDisabledAndNotStagingOrProduction(this IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _ = bool.TryParse(configuration["Auth:DisableAuth"], out var disableAuth);

        return disableAuth && !(webHostEnvironment.IsProduction() || webHostEnvironment.IsStaging());
    }
}

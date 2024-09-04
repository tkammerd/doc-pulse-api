using Ots.Auth.ApiComponents.Configuration;

namespace Doc.Pulse.Api.Setup.Auth
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddOtsAuthentication(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _ = bool.TryParse(configuration["Auth:UseLocalhost"], out var useLocalhost);

            services.AddOtsIdentityAuthentication((opt) =>
            {
                opt.DevelopmentMode = environment.IsDevelopment();
                opt.ApiName = configuration["Auth:OtsIdentity:ApiName"];
                opt.AuthorityUrl = !useLocalhost ? configuration["Auth:OtsIdentity:Authority"] : configuration["Auth:OtsIdentity:AuthorityLocalhost"];
                opt.UseCertificateForwarding = configuration.UseCertificateForwarding();
                opt.CertificateForwardingHeader = configuration["Auth:OtsIdentity:CertificateForwardingHeader"];
            });

            return services;
        }
    }
}

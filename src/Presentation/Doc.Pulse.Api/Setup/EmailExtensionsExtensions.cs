using Esb.CustomerCommunication;
using Esb.DelegatR;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Infrastructure.Extensions;
using Doc.Pulse.Infrastructure.Services;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Doc.Pulse.Api.Setup
{
    public static class EmailExtensionsExtensions
    {
        public static IServiceCollection AddEsbEmailing(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            // ... instead using the existing register MeditR code and adding --> typeof(ICcClient).Assembly
            //foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            //}

            string strategy = configuration["CC:SigningCertificate:Strategy"] ?? string.Empty;
            string identifier = configuration["CC:SigningCertificate:Identifier"] ?? string.Empty;
            string secret = configuration["CC:SigningCertificate:Secret"] ?? string.Empty;
            string storeName = configuration["CC:SigningCertificate:StoreName"] ?? string.Empty;
            string storeLocation = configuration["CC:SigningCertificate:StoreLocation"] ?? string.Empty;

            var eaCertificate = environment.LookupCertificate(strategy, identifier, secret, storeName, storeLocation, X509KeyStorageFlags.Exportable);


            var ccEmailService = configuration["CC:CcEmailService"];
            //var eaCertificate = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpper()) == "LOCAL" ?
            //    new X509Certificate2(@"Certificates/svc-cc-modpoc-dev.pfx", "Christmas!", X509KeyStorageFlags.Exportable) :
            //    new X509Certificate2(configuration["OtsCertificates:EA"], configuration["OtsCertificates:EAPassword"], X509KeyStorageFlags.Exportable);

            var saml2provider = configuration["CC:Saml2ProviderUri"];
            var costCode = configuration["CC:CostAllocationCode"];
            var senderEmailAddress = configuration["CC:SenderEmailAddress"];
            var senderDisplay = configuration["CC:SenderDisplay"];
            var smtpAccount = configuration["CC:SmtpAccount"];

            services.AddEsbDelegatR(options =>
            {
                options.ESBProviderUris = new Dictionary<string, string>()
                    {
                        {CcEsbSoapServices.CcEmailService, ccEmailService! },
                    };
                options.Saml2ProviderUri = saml2provider;
                options.X509Certificate2 = eaCertificate;
                options.AccountCode = costCode;
            }).AddCc(options =>
            {
                options.SenderEmailAddress = senderEmailAddress;
                options.SenderDisplayName = senderDisplay;
                options.SmtpAccount = smtpAccount;
            });

            //services.AddAutoMapper(Assembly.GetAssembly(typeof(EmailMappingProfile)));

            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}

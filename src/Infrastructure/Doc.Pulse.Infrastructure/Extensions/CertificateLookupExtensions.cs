using AppDmDoc.SharedKernel.Core.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Doc.Pulse.Core.Trouble.Exceptions;
using System.Security.Cryptography.X509Certificates;

namespace Doc.Pulse.Infrastructure.Extensions;

public static class CertificateLookupExtensions
{
    public static X509Certificate2? LookupCertificate(this IWebHostEnvironment environment, string strategy, string identifier, string secret, string storeName, string storeLocation, X509KeyStorageFlags flags = default)
    {
        try
        {
            if (Enum.TryParse(strategy, true, out X509FindType findType))
            {
                var cert = GetCert(
                    identifier,
                    storeName.TryParseEnumOrDefault(StoreName.My, true),
                    storeLocation.TryParseEnumOrDefault(StoreLocation.LocalMachine, true),
                    findType,
                    false
                    );

                return cert;
            }
            else if (strategy?.ToLower() == "file")
            {
                if (environment.IsProduction() || environment.IsStaging())
                    throw new CertificateLookupException(strategy, identifier, "Do not configure the certificate in this way for production. Add to store instead.");
                if (!File.Exists(identifier))
                    throw new FileNotFoundException($"Certificate File {identifier} indicated in appsettings.json could not be found.");

                X509Certificate2? cert = default;
                if (flags == default)
                {
                    if (string.IsNullOrEmpty(secret))
                        cert = new X509Certificate2(identifier);
                    else
                        cert = new X509Certificate2(identifier, secret);
                }

                if (string.IsNullOrEmpty(secret))
                    cert = new X509Certificate2(identifier, (string?)null, flags);
                else
                    cert = new X509Certificate2(identifier, secret, flags);

                return cert;
            }
            else if (strategy?.ToLower() == "ignore")
            {
                return null;
            }
            else
            {
                throw new CertificateLookupException(strategy, identifier, $"Invalid Signing Strategy Indicated for IdentityServer Signing Certification '{strategy} is not valid'");
            }
        }
        catch (CertificateLookupException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new CertificateLookupException(strategy, identifier, $"Unexpected exception during certificate lookup. See inner exception.", e);
        }
    }


    private static X509Certificate2 GetCert(string value, StoreName storeName, StoreLocation storeLocation, X509FindType x509FindType, bool validOnly = true, bool hasPrivateKey = true, bool isEcdsa = false)
    {
        using X509Store x509Store = new(storeName, storeLocation);

        x509Store.Open(OpenFlags.ReadOnly);
        X509Certificate2Collection x509Certificate2Collection = x509Store.Certificates.Find(x509FindType, value, validOnly);
        x509Store.Close();

        if (x509Certificate2Collection.Count == 0)
        {
            throw new InvalidOperationException("Service Provider certificate could not be found.");
        }

        if (x509Certificate2Collection.Count > 1)
        {
            throw new InvalidOperationException("Multiple Service Provider certificates were found, must only provide one.");
        }


        X509Certificate2 x509Certificate = x509Certificate2Collection[0];
        if (!isEcdsa)
        {
            if (x509Certificate.GetRSAPrivateKey() == null && hasPrivateKey)
            {
                throw new InvalidOperationException("The certificate for this service provider has no private key.");
            }
        }
        else if (x509Certificate.GetECDsaPrivateKey() == null && hasPrivateKey)
        {
            throw new InvalidOperationException("The certificate for this service provider has no private key.");
        }

        return x509Certificate; //new X509Certificate2(x509Certificate);
    }
}

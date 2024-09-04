using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Authentication;

namespace Doc.Pulse.Api.Setup;

public static class WebApplicationExtensions
{
    public static WebApplication DebugAuthSetBreakpointInHere(this WebApplication app)
    {
        if (app.Environment.IsDevelopment()) {
            app.Use(async (ctx, next) =>
            {
                var url = ctx.Request.Path.Value;
                var isAuthenticated = ctx.User.Identity?.IsAuthenticated ?? false;

                if (isAuthenticated)
                {
                    // <--- set a breakpoint here to debug
                }

                await next();
            });
        }

        return app;
    }

    public static WebApplicationBuilder ConfigureKestrelToAllowSelfSignedDevCertForOtsAuth(this WebApplicationBuilder builder)
    {
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.AllowSynchronousIO = true;
            options.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.CheckCertificateRevocation = false;
                httpsOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                httpsOptions.SslProtocols = SslProtocols.Tls12;

                if (builder.Environment.IsDevelopment())
                {
                    httpsOptions.AllowAnyClientCertificate();
                }
            });
        });

        return builder;
    }
}

using Ots.Auth.ApiComponents.Configuration;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Infrastructure.Services;

namespace Doc.Pulse.Api.Setup.Auth
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddOtsAuthorization(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            var permissionClaimType = configuration["Auth:OtsIdentity:OtsPermissionsClaimType"] ?? string.Empty;
            var anyUserClaims = (configuration["Auth:PortalUserRoles"] ?? string.Empty).Split(',').ToList();


            services.AddOtsIdentityAuthorization((opts) =>
            {
                opts.PermissionClaimType = permissionClaimType;
                opts.AuthorityUrl = configuration["Auth:OtsIdentity:Authority"];
            });

            services.AddScoped<IResolveUserService, ResolveUserServiceByHttpContext>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Role_Arc_Support",
                    policy => {
                        policy.RequireAssertion(context => context.User.HasClaim(c =>
                           c.Value == "ARC-Support"
                        ));
                    });

                options.AddPolicy("Role_Arc_Manager",
                    policy => {
                        policy.RequireAssertion(context => context.User.HasClaim(c =>
                            c.Value == "ARC-Support"
                            || c.Value == "ARC-Manager"));
                });

                options.AddPolicy("Role_Arc_Reader",
                    policy => {
                        policy.RequireAssertion(context => context.User.HasClaim(c =>
                            c.Value == "ARC-Support"
                            || c.Value == "ARC-Manager"
                            || c.Value == "ARC-Reader"
                        ));
                    });
            });


            return services;
        }
    }
}

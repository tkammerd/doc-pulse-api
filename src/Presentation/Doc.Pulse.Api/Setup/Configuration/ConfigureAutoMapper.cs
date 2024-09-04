using AutoMapper;
using Doc.Pulse.Api.Setup.Configuration;

namespace Doc.Pulse.Api.Setup.Configuration
{
    public static class ConfigureAutoMapper
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(c => c.AddProfile<AutoMapperProfile>(), typeof(Program).Assembly);
            return services;
        }
    }
}

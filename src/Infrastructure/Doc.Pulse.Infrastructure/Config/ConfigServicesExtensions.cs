using FluentValidation;
using FluentValidation.AspNetCore;
using AppDmDoc.SharedKernel.Core.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Doc.Pulse.Core.Abstractions;
using Doc.Pulse.Core.Entities._Kernel;
using Doc.Pulse.Core.Helpers;
using Doc.Pulse.Infrastructure.Services;
using Doc.Pulse.Infrastructure.Util;
using Ots.AppDmDoc.Abstractions.AutoMapper;
using Ots.AppDmDoc.Abstractions.HashIds;
using OtsLogger;
using Serilog;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using Ots.AppDmDoc.Adapters.Adapter.HashIds;
using Ots.AppDmDoc.Adapters.Adapter.Mapper;

namespace Doc.Pulse.Infrastructure.Config;

public static class ConfigServicesExtensions
{
    private static Assembly[] _internalAssemblies = [typeof(ConfigServicesExtensions).Assembly, typeof(MediatrResultBase).Assembly];

    public static IServiceCollection ConfigureAllDocSharedKernelServices(this IServiceCollection services, ConfigurationManager configuration, params Assembly[] assemblies)
    {
        services.ConfigureCoreServices()
            .ConfigureDocAutoMapper(assemblies)
            .ConfigureDocMediatR(assemblies)
            .ConfigureDocHashIds(configuration)
            .AddDocOtsLogger(configuration)
            .AddDocFluentValidation(assemblies)
            ;

        return services;
    }

    public static IServiceCollection ConfigureCoreServices(this IServiceCollection services)
    {
        //services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<ICurrentUserService, CurrentUserServiceViaHttpContext>();

        return services;
    }

    public static IServiceCollection ConfigureDocMediatR(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMediatR(cfg =>
             cfg.RegisterServicesFromAssemblies([.. assemblies, .. _internalAssemblies])
        );

        return services;
    }

    public static IServiceCollection ConfigureDocAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
    {
        //services.AddAutoMapper(typeof(Program).Assembly, typeof(EncodeHashIdResolver<>).Assembly, typeof(ContractBase<>).Assembly);
        services.AddAutoMapper(assemblies.Concat(_internalAssemblies).ToArray())
            .AddScoped<IMapperAdapter, MapperBaseAdapter>();

        return services;
    }

    public static IServiceCollection ConfigureDocHashIds(this IServiceCollection services, IConfiguration configuration, string sectionName = "HashIdsAdaptorConfig")
    {
        var configurationSection = configuration.GetSection(sectionName);

        services.Configure<HashIdsAdaptorConfig>(configurationSection)
            .AddSingleton(typeof(IHashIdsAdaptor<>), typeof(HashIdsAdaptor<>))
            .AddSingleton(typeof(IHashIdsAdaptor), typeof(HashIdsAdaptor));

        return services;
    }

    public static IServiceCollection AddDocOtsLogger(this IServiceCollection services, ConfigurationManager configuration)
    {
        Log.Logger = new LoggerConfiguration()
            //.Enrich.FromLogContext()
            //.ReadFrom.Configuration(builder.Configuration)
            .AddOtsConfiguration(configuration)
            .CreateLogger();
        services.AddOtsLogger(opt => { opt.ObjectGuid = "Uid"; });

        //services.AddOtsLogger(opt => { opt.ObjectGuid = ClaimTypes.NameIdentifier; opt.RegisterMediatrMiddleware = false; });
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DefaultDocMediatrLogging<,>));

        return services;
    }

    public static IServiceCollection AddDocFluentValidation(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach (Assembly assembly in assemblies)
            services.AddValidatorsFromAssembly(assembly);

        services
            //.AddValidatorsFromAssemblyContaining<Program>()
            .AddFluentValidationAutoValidation(fv =>
            {
                //fv.DisableDataAnnotationsValidation = true;
            })
            .AddFluentValidationClientsideAdapters();

        services.AddTransient<IValidatorInterceptor, DocFluentErrorModelInterceptor>();

        return services;
    }

    public static IMvcBuilder AddValidationFailedBehavior(this IMvcBuilder services)
    {
        services.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => !e.ErrorMessage.IsValidJson()
                        ? new ApiError("", e.ErrorMessage)
                        : JsonSerializer.Deserialize<ApiError>(e.ErrorMessage) ?? new ApiError("", e.ErrorMessage)
                    );

                var respWrapper = ApiResponseFactory.Fail(HttpStatusCode.BadRequest, "Validation Failed - One or more errors indicated.", errors);
                //var respWrapper = new ApiResponse()
                //{
                //    StatusCode = HttpStatusCode.BadRequest,
                //    ResponseUid = Guid.NewGuid(),
                //    Message = "Validation Failed - One or more errors indicated.",
                //    IsSuccess = false,
                //    Errors = errors
                //};

                return new ObjectResult(respWrapper);
            };
        });

        return services;
    }
}

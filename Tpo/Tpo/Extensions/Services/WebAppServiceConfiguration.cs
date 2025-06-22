using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Serilog;
using SharedKernel.Services;
using System.Reflection;
using System.Text.Json.Serialization;
using Tpo.Middleware;

namespace Tpo.Extensions.Services;

public static class WebAppServiceConfiguration
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(Log.Logger);
        builder.Services.AddProblemDetails(ProblemDetailsConfigurationExtension.ConfigureProblemDetails)
            .AddProblemDetailsConventions();

        // update CORS for your env
        builder.Services.AddCorsService("CorsPolicy", builder.Environment);

        // SERVICIOS DE INFRAESTRUCTURA (DATABASES, WEB SERVICES, etc.)
        builder.Services.AddInfrastructure(builder.Environment, builder.Configuration);

        builder.Services.AddControllers()
            .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddApiVersioningExtension();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // registers all services that inherit from your base service interface: IScopedService
        builder.Services.AddBoundaryServices(Assembly.GetExecutingAssembly());

        builder.Services.AddMvc();

        builder.Services.AddHealthCheckExtension(builder.Configuration);
        builder.Services.AddSwaggerExtension(builder.Configuration);

        // registers all validators
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    ///     Registers all services in the assembly of the given interface.
    /// </summary>
    private static void AddBoundaryServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (!assemblies.Any())
            throw new ArgumentException(
                "No assemblies found to scan. Supply at least one assembly to scan for handlers.");

        foreach (var assembly in assemblies)
        {
            var rules = assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(IScopedService)) ==
                    typeof(IScopedService));

            foreach (var rule in rules)
                foreach (var @interface in rule.GetInterfaces())
                    services.Add(new ServiceDescriptor(@interface, rule, ServiceLifetime.Scoped));
        }
    }
}
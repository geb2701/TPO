using HealthChecks.ApplicationStatus.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Template.Extensions.Services;

public static class HealthCheckExtension
{
    public static void AddHealthCheckExtension(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuración de HealthChecks
        services.AddHealthChecks()
            .AddApplicationStatus("api_status", tags: new[] { "api" });
        //.AddSqlServer(
        //    configuration.GetConnectionStringOptions().Template,
        //    name: "sql",
        //    failureStatus: HealthStatus.Degraded,
        //    tags: new[] { "db", "sql", "sqlserver" });

        // Custom HealthCheck
        //services.AddHealthChecks().AddCheck<ServerHealthCheck>("server_health_check", tags: new []{"custom", "api"});  

        // HealthChecks UI
        services.AddHealthChecksUI(setup =>
        {
            setup.SetHeaderText("BNA.ARI.Template - Health Checks Status");
            setup.AddHealthCheckEndpoint("API", "/healthz");
        }).AddInMemoryStorage();
    }
}

public class ServerHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        var isServerHealthy = CheckServerStatus();

        return Task.FromResult(isServerHealthy
            ? HealthCheckResult.Healthy("El servidor está en funcionamiento.")
            : HealthCheckResult.Unhealthy("El servidor no está respondiendo."));
    }

    private bool CheckServerStatus()
    {
        // Implementar la lógica para verificar el estado del servidor externo.
        return true;
    }
}
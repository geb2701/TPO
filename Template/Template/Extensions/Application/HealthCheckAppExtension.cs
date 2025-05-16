using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SharedKernel;

namespace Template.Extensions.Application;

public static class HealthCheckAppExtension
{
    public static void UseHealthCheck(this WebApplication app, IWebHostEnvironment env)
    {
        if (!env.IsEnvironment(Consts.Testing.FunctionalTestingEnvName))
        {
            app.MapHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.MapHealthChecksUI(setup => setup.AddCustomStylesheet("dotnet.css"));
        }
    }
}
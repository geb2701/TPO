using SharedKernel;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Template.Extensions.Application;

public static class SwaggerAppExtension
{
    public static void UseSwaggerExtension(this WebApplication app, IConfiguration configuration,
        IWebHostEnvironment env)
    {
        if (!env.IsEnvironment(Consts.Testing.FunctionalTestingEnvName))
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                config =>
                {
                    var descriptions = app.DescribeApiVersions();
                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        config.SwaggerEndpoint(url, name);
                    }

                    config.DocExpansion(DocExpansion.None);
                });
        }
    }
}
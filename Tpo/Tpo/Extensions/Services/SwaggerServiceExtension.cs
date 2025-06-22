using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tpo.Configurations;

namespace Tpo.Extensions.Services;

public static class SwaggerServiceExtension
{
    public static void AddSwaggerExtension(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(config =>
        {
            config.CustomSchemaIds(type => type.ToString().Replace("+", "."));
            config.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });

            config.IncludeXmlComments(string.Format(
                @$"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}Tpo.WebApi.xml"));
        });
    }
}

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Version = description.ApiVersion.ToString(),
                Title = "BNA.Tpo.Api",
                Description = ""
            });
    }
}
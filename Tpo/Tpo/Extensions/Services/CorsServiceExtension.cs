namespace Tpo.Extensions.Services;

public static class CorsServiceExtension
{
    public static void AddCorsService(this IServiceCollection services, string policyName, IWebHostEnvironment env)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, builder =>
                builder.SetIsOriginAllowed(IsOriginAllowed)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("X-Pagination"));
        });
    }

    private static bool IsOriginAllowed(string origin)
    {
        var origins = new List<string>()
        {
            ".net",
            ".com",
            "localhost"
        };
        return origins.Any(y => origin.Contains(y));
    }
}

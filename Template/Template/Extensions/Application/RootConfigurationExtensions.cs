namespace Template.Extensions.Application;

public static class RootConfigurationExtensions
{
    public static string GetJaegerHostValue(this IConfiguration configuration)
    {
        return configuration.GetSection("JaegerHost").Value;
    }
}
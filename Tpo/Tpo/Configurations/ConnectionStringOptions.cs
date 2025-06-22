namespace Tpo.Configurations;

public class ConnectionStringOptions
{
    public const string SectionName = "ConnectionStrings";
    public const string TpoKey = "Tpo";

    public string Tpo { get; set; } = string.Empty;
}

public static class ConnectionStringOptionsExtensions
{
    public static ConnectionStringOptions GetConnectionStringOptions(this IConfiguration configuration)
    {
        return configuration.GetSection(ConnectionStringOptions.SectionName).Get<ConnectionStringOptions>();
    }
}
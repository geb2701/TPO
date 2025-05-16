namespace Template.Configurations;

public class ConnectionStringOptions
{
    public const string SectionName = "ConnectionStrings";
    public const string TemplateKey = "Template";

    public string Template { get; set; } = string.Empty;
}

public static class ConnectionStringOptionsExtensions
{
    public static ConnectionStringOptions GetConnectionStringOptions(this IConfiguration configuration)
    {
        return configuration.GetSection(ConnectionStringOptions.SectionName).Get<ConnectionStringOptions>();
    }
}
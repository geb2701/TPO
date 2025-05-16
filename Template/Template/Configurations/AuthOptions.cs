namespace Template.Configurations;

public class AuthOptions
{
    public const string SectionName = "Auth";

    public string Audience { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
    public string AuthorizationUrl { get; set; } = string.Empty;
    public string TokenUrl { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}

public static class AuthOptionsExtensions
{
    public static AuthOptions GetAuthOptions(this IConfiguration configuration)
    {
        return configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>();
    }
}
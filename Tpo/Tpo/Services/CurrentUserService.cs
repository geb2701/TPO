using SharedKernel.Services;
using System.Security.Claims;

namespace Tpo.Services;

public interface ICurrentUserService : IScopedService
{
    ClaimsPrincipal? User { get; }
    string? UserId { get; }
    string? Email { get; }
    string? FirstName { get; }
    string? LastName { get; }
    string? Username { get; }
    string? ClientId { get; }
    bool IsMachine { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
    public string? UserId => User?.FindFirstValue(ClaimTypes.Name);
    public string? Email => User?.FindFirstValue(ClaimTypes.Email);
    public string? FirstName => User?.FindFirstValue(ClaimTypes.GivenName);
    public string? LastName => User?.FindFirstValue(ClaimTypes.Surname);

    public string? Username => User
        ?.Claims
        ?.FirstOrDefault(x => x.Type is "preferred_username" or "username")
        ?.Value;

    public string? ClientId => User
        ?.Claims
        ?.FirstOrDefault(x => x.Type is "client_id" or "clientId")
        ?.Value;

    public bool IsMachine => ClientId != null;
}
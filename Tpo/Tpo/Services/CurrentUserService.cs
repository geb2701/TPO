using SharedKernel.Services;
using System.Security.Claims;
using Tpo.Domain.User;

namespace Tpo.Services;

public interface ICurrentUserService : IScopedService
{
    User GetUser();
}

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public User GetUser()
    {
        try
        {
            httpContextAccessor!.HttpContext!.Items.TryGetValue("User", out var _user);
            var user = _user as User;
            return user!;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tpo.Domain.User;

namespace Tpo.Attributes;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute() : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Items.TryGetValue("User", out var _user) || _user is not User user || user.Name is null)
        {
            context.Result = new JsonResult(new { message = "User Not Found" })
            { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        return;
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tpo.Domain.Usuario;

namespace Tpo.Attributes;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute() : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Items.TryGetValue("Usuario", out var _user) || _user is not Usuario user || user.Alias is null)
        {
            context.Result = new JsonResult(new { message = "Usuario Not Found" })
            { StatusCode = StatusCodes.Status401Unauthorized };
            return;
        }

        return;
    }
}
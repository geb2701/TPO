
using Tpo.Domain.User;
using Tpo.Services.Jwt;

namespace Tpo.Middleware;

public class AuthMiddleware(RequestDelegate next)
{
    private static readonly User CurrentUser = User.Create(new Domain.User.Models.UserForCreation());
    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
    {

        var token = context.Request.Headers["JwtAuthorization"].FirstOrDefault()?.Split(" ").Last();
        if (await jwtUtils.ValidationJwtTokenAsync(token))
        {
            await jwtUtils.LoadToken(token!, CurrentUser!);
        }
        
        context.Items["User"] = CurrentUser;

        await next(context);
    }
}
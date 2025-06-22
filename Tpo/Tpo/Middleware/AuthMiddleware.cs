
using Tpo.Domain.Usuario;
using Tpo.Services.Jwt;

namespace Tpo.Middleware;

public class AuthMiddleware(RequestDelegate next)
{
    private static readonly Usuario CurrentUsuario = Usuario.Create(new Domain.Usuario.Models.UsuarioForCreation());
    public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["JwtAuthorization"].FirstOrDefault()?.Split(" ").Last();
        if (await jwtUtils.ValidationJwtTokenAsync(token))
        {
            await jwtUtils.LoadToken(token!, CurrentUsuario!);
        }
        
        context.Items["Usuario"] = CurrentUsuario;

        await next(context);
    }
}
using SharedKernel.Services;
using System.Security.Claims;
using Tpo.Domain.Usuario;

namespace Tpo.Services;

public interface ICurrentUsuarioService : IScopedService
{
    Usuario GetUsuario();
}

public class CurrentUsuarioService(IHttpContextAccessor httpContextAccessor) : ICurrentUsuarioService
{
    public Usuario GetUsuario()
    {
        try
        {
            httpContextAccessor!.HttpContext!.Items.TryGetValue("Usuario", out var _user);
            var user = _user as Usuario;
            return user!;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
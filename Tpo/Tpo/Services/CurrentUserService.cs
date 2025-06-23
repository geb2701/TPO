using SharedKernel.Services;
using Tpo.Domain.Usuario;

namespace Tpo.Services;

public interface ICurrentUsuarioService : IScopedService
{
    int GetUsuarioId();
    string GetUsuarioNombre();
}

public class CurrentUsuarioService(IHttpContextAccessor httpContextAccessor) : ICurrentUsuarioService
{
    public int GetUsuarioId()
    {
        try
        {
            httpContextAccessor!.HttpContext!.Items.TryGetValue("Usuario", out var _user);
            var user = _user as Usuario;

            return user.Id;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public string GetUsuarioNombre()
    {
        try
        {
            httpContextAccessor!.HttpContext!.Items.TryGetValue("Usuario", out var _user);
            var user = _user as Usuario;

            return user.UsuarioNombre;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
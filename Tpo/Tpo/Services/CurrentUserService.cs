using SharedKernel.Services;
using Tpo.Domain.Usuario;

namespace Tpo.Services;

public interface ICurrentUsuarioService : IScopedService
{
    int GetUsuarioId();
    string GetAlias();
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
    public string GetAlias()
    {
        try
        {
            httpContextAccessor!.HttpContext!.Items.TryGetValue("Usuario", out var _user);
            var user = _user as Usuario;

            return user.Alias;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
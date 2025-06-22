using SharedKernel.Databases;
using Tpo.Databases;

namespace Tpo.Domain.UsuarioDeporte.Services;

public class UsuarioDeporteRepository : Repository<UsuarioDeporte>, IUsuarioDeporteRepository
{
    public UsuarioDeporteRepository(TpoDbContext context) : base(context)
    {
    }
}

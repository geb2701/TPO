using Tpo.Databases;
using Tpo.Databases.Repositorys;

namespace Tpo.Domain.UsuarioDeporte.Services;

public interface IUsuarioDeporteRepository : IRepositoryIncludableQueryable<UsuarioDeporte, int>
{
}

public sealed class UsuarioDeporteRepository(TpoDbContext dbContext) : GenericRepository<UsuarioDeporte, int>(dbContext), IUsuarioDeporteRepository
{
}
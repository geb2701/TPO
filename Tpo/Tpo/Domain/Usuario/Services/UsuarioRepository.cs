using Tpo.Databases;
using Tpo.Databases.Repositorys;

namespace Tpo.Domain.Usuario.Services;

public interface IUsuarioRepository : IRepositoryIncludableQueryable<Usuario, int>
{
}

public sealed class UsuarioRepository(TpoDbContext dbContext) : GenericRepository<Usuario, int>(dbContext), IUsuarioRepository
{
}
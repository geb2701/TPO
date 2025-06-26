using Tpo.Databases;
using Tpo.Databases.Repositorys;

namespace Tpo.Domain.Partido.Services;

public interface IPartidoRepository : IRepositoryIncludableQueryable<Partido, int>
{
}

public sealed class PartidoRepository(TpoDbContext dbContext) : GenericRepository<Partido, int>(dbContext), IPartidoRepository
{
}
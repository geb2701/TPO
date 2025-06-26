using Tpo.Databases;
using Tpo.Databases.Repositorys;

namespace Tpo.Domain.Jugador.Services;

public interface IJugadorRepository : IRepositoryIncludableQueryable<Jugador, int>
{
}

public sealed class JugadorRepository(TpoDbContext dbContext) : GenericRepository<Jugador, int>(dbContext), IJugadorRepository
{
}
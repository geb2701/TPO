using Tpo.Databases;
using Tpo.Databases.Repositorys;

namespace Tpo.Domain.Deporte.Services;

public interface IDeporteRepository : IRepositoryIncludableQueryable<Deporte, int>
{
}

public sealed class DeporteRepository(TpoDbContext dbContext) : GenericRepository<Deporte, int>(dbContext), IDeporteRepository
{
}
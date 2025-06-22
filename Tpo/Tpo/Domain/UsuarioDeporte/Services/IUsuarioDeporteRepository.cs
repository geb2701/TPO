using SharedKernel.Databases;
using Tpo.Databases;
using Tpo.Databases.Repositorys;

namespace Tpo.Domain.Deporte.Services;

public interface IDeporteRepository : IGenericRepository<Deporte, int>
{
}

public sealed class ServicesRepository(TpoDbContext dbContext) : GenericRepository<Deporte, int>(dbContext), IDeporteRepository
{
}
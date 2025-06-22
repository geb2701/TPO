using SharedKernel.Databases;
using Tpo.Databases;
using Tpo.Databases.Repositorys;

namespace Tpo.Domain.User.Services;

public interface IUserRepository : IGenericRepository<User, int>
{
}

public sealed class UserRepository(TpoDbContext dbContext) : GenericRepository<User, int>(dbContext), IUserRepository
{
}
using SharedKernel.Databases;
using Tpo.Databases;
using Tpo.Databases.Repositorys;

namespace Tpo.Domain.User.Services;

public interface IUserRepository : IGenericRepository<User, int>
{
}

public sealed class UserRepository : GenericRepository<User, int>, IUserRepository
{
    private readonly TpoDbContext _dbContext;

    public UserRepository(TpoDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
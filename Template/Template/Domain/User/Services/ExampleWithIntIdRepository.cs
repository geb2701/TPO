using SharedKernel.Databases;
using Template.Databases;
using Template.Databases.Repositorys;

namespace Template.Domain.User.Services;

public interface IUserRepository : IGenericRepository<User, int>
{
}

public sealed class UserRepository : GenericRepository<User, int>, IUserRepository
{
    private readonly TemplateDbContext _dbContext;

    public UserRepository(TemplateDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
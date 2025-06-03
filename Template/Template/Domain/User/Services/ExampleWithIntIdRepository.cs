using SharedKernel.Databases;
using Template.Databases;
using Template.Databases.Repositorys;

namespace Template.Domain.User.Services;

public interface IExampleWithIntIdRepository : IGenericRepository<User, int>
{
}

public sealed class ExampleWithIntIdRepository : GenericRepository<User, int>, IExampleWithIntIdRepository
{
    private readonly TemplateDbContext _dbContext;

    public ExampleWithIntIdRepository(TemplateDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
using SharedKernel.Databases;
using Template.Databases;
using Template.Databases.Repositorys;

namespace Template.Domain.ExampleWithIntId.Services;

public interface IExampleWithIntIdRepository : IGenericRepository<ExampleWithIntId, int>
{
}

public sealed class ExampleWithIntIdRepository : GenericRepository<ExampleWithIntId, int>, IExampleWithIntIdRepository
{
    private readonly TemplateDbContext _dbContext;

    public ExampleWithIntIdRepository(TemplateDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
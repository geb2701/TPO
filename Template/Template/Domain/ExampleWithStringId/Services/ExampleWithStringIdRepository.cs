using SharedKernel.Databases;
using Template.Databases;
using Template.Databases.Repositorys;

namespace Template.Domain.ExampleWithStringId.Services;

public interface IExampleWithStringIdRepository : IGenericRepository<ExampleWithStringId, string>
{
}

public sealed class ExampleWithStringIdRepository : GenericRepository<ExampleWithStringId, string>, IExampleWithStringIdRepository
{
    private readonly TemplateDbContext _dbContext;

    public ExampleWithStringIdRepository(TemplateDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
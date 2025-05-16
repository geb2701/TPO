using SharedKernel.Databases;

namespace Template.Databases;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TemplateDbContext _dbContext;

    public UnitOfWork(TemplateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CommitChanges(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
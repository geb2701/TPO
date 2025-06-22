using SharedKernel.Databases;

namespace Tpo.Databases;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TpoDbContext _dbContext;

    public UnitOfWork(TpoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> CommitChanges(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
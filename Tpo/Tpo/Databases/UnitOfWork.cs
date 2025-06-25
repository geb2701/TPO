using SharedKernel.Databases;

namespace Tpo.Databases;

public sealed class UnitOfWork(TpoDbContext dbContext) : IUnitOfWork
{
    public async Task<int> CommitChanges(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> SystemCommitChanges(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesSystemAsync(cancellationToken);
    }
}
using SharedKernel.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Databases
{
    public interface IUnitOfWork : IScopedService
    {
        Task<int> CommitChanges(CancellationToken cancellationToken = default);
        Task<int> SystemCommitChanges(CancellationToken cancellationToken = default);
    }
}

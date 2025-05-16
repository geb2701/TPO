using SharedKernel.Domain.Entity;

namespace Template.Databases.Repositorys
{
    public interface IApprovableRepository<TEntity, TKey> where TEntity : Entity<TKey>
    {

    }
    public abstract class ApprovableRepository<TEntity, TKey> : GenericRepository<TEntity, TKey>, IApprovableRepository<TEntity, TKey> where TEntity : ApprovableEntity<TKey>
    {
        private readonly TemplateDbContext _dbContext;

        protected ApprovableRepository(TemplateDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

using SharedKernel.Domain.Entity;
using Template.Domain.Attachments;
using Template.Domain.Attachments.Services;

namespace Template.Databases.Repositorys
{
    public interface IAttachmentsApprovableVersionableRepository<TEntity, Guid> : IAttachmentOwnerRepository<TEntity> where TEntity : Entity<Guid>
    {

    }
    public abstract class AttachmentsApprovableVersionableRepository<TEntity> : ApprovableVersionableRepository<TEntity, Guid>, IAttachmentsApprovableVersionableRepository<TEntity, Guid> where TEntity : ApprovableVersionableEntity<Guid>
    {
        private readonly TemplateDbContext _dbContext;

        protected AttachmentsApprovableVersionableRepository(TemplateDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Attachment> GetFiles(TEntity entity)
        {
            return _dbContext.Attachments
                .Where(x =>
                    x.OwnerId == entity.Id &&
                    x.OwnerType == typeof(TEntity).Name
                );
        }
    }
}

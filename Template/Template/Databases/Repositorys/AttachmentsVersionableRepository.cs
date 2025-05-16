using Template.Domain.Attachments;
using Template.Domain.Attachments.Abstractions;
using Template.Domain.Attachments.Services;

namespace Template.Databases.Repositorys;

public interface IAttachmentsVersionableRepository<TEntity> : IVersionableRepository<TEntity, Guid>, IAttachmentOwnerRepository<TEntity>
    where TEntity : AttachmentsVersionableEntity
{
}

public abstract class AttachmentsVersionableRepository<TEntity> : VersionableRepository<TEntity, Guid>,
    IAttachmentsVersionableRepository<TEntity>
    where TEntity : AttachmentsVersionableEntity
{
    private readonly TemplateDbContext _dbContext;

    protected AttachmentsVersionableRepository(TemplateDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Attachment> GetFiles(TEntity entity)
    {
        return _dbContext.Attachments
            .Where(x =>
                x.OwnerId == entity.OriginKey &&
                x.OwnerType == typeof(TEntity).Name &&
                x.FromVersion <= entity.Version &&
                (x.ToVersion == null || x.ToVersion >= entity.Version)
            );
    }
}
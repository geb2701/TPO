using SharedKernel.Databases;
using Template.Domain.Attachments;
using Template.Domain.Attachments.Abstractions;
using Template.Domain.Attachments.Services;

namespace Template.Databases.Repositorys;

public interface IAttachmentsApprovableRepository<TEntity> : IGenericRepository<TEntity, Guid>, IAttachmentOwnerRepository<TEntity>
    where TEntity : AttachmentsApprovableEntity
{
}

public abstract class AttachmentsApprovableRepository<TEntity> : GenericRepository<TEntity, Guid>,
    IAttachmentsApprovableRepository<TEntity>
    where TEntity : AttachmentsApprovableEntity
{
    private readonly TemplateDbContext _dbContext;

    protected AttachmentsApprovableRepository(TemplateDbContext dbContext) : base(dbContext)
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
namespace Template.Domain.Attachments.Services;

public interface IAttachmentOwnerRepository<TEntity> where TEntity : class
{
    IQueryable<Attachment> GetFiles(TEntity entity);
}
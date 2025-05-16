using SharedKernel.Databases;
using Template.Databases;
using Template.Databases.Repositorys;

namespace Template.Domain.Attachments.Services;

public interface IAttachmentRepository : IGenericRepository<Attachment, Guid>
{
}

public sealed class AttachmentRepository : GenericRepository<Attachment, Guid>, IAttachmentRepository
{
    private readonly TemplateDbContext _dbContext;

    public AttachmentRepository(TemplateDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
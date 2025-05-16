using Newtonsoft.Json;
using SharedKernel.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using Template.Domain.Attachments.DomainEvents;
using Template.Domain.Attachments.Models;

namespace Template.Domain.Attachments;

public class Attachment : BaseEntity<Guid>
{
    protected Attachment()
    {
    } // For EF + Mocking

    [Required] public string FileName { get; private set; }
    [Required] public int Size { get; private set; }
    [Required] public byte[] Content { get; private set; }
    [Required] public string FileType { get; private set; }
    public int? FromVersion { get; private set; }
    public int? ToVersion { get; private set; }
    public Guid? OwnerId { get; private set; }
    public string? OwnerType { get; private set; }

    public static async Task<Attachment> Create(AttachmentForCreation attachmentForCreation)
    {
        var newAttachment = new Attachment();

        byte[] fileBytes;
        using (var stream = new MemoryStream())
        {
            await attachmentForCreation.File.CopyToAsync(stream);
            fileBytes = stream.ToArray();
        }

        newAttachment.Content = fileBytes;
        newAttachment.FileName = attachmentForCreation.File.FileName;
        newAttachment.Size = (int)attachmentForCreation.File.Length;
        newAttachment.FileType = attachmentForCreation.File.ContentType;

        newAttachment.QueueDomainEvent(new AttachmentCreated { Attachment = newAttachment });

        return newAttachment;
    }

    public Attachment Update(AttachmentForUpdate attachmentForUpdate)
    {
        FromVersion = attachmentForUpdate.FromVersion;
        ToVersion = attachmentForUpdate.ToVersion;
        OwnerId = attachmentForUpdate.OwnerId;
        OwnerType = attachmentForUpdate.OwnerType;

        QueueDomainEvent(new AttachmentUpdated { Id = Id });
        return this;
    }

    public void UpdateToVersion(int toVersion)
    {
        ToVersion = toVersion;
    }

    public void Delete()
    {
        UpdateIsDeleted(true);
    }
}
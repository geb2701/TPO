using SharedKernel.Domain;

namespace Template.Domain.Attachments.DomainEvents;

public sealed class AttachmentCreated : DomainEvent
{
    public Attachment Attachment { get; set; }
}
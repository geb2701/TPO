using SharedKernel.Domain;

namespace Template.Domain.Attachments.DomainEvents;

public sealed class AttachmentUpdated : DomainEvent
{
    public Guid Id { get; set; }
}
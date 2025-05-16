namespace Template.Domain.Attachments.Models;

public sealed record AttachmentForUpdate
{
    public int? FromVersion { get; set; }
    public int? ToVersion { get; set; }
    public Guid OwnerId { get; set; }
    public string OwnerType { get; set; }
}
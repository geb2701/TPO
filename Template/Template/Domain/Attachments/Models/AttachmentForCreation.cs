namespace Template.Domain.Attachments.Models;

public sealed record AttachmentForCreation
{
    public IFormFile File { get; set; }
}
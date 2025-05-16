namespace Template.Domain.Attachments.Dtos;

public sealed record AttachmentForCreationDto
{
    public IFormFile File { get; set; }
}
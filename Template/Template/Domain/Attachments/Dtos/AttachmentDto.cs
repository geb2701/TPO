namespace Template.Domain.Attachments.Dtos;

public sealed record AttachmentDto
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    public string FileName { get; set; }
    public int Size { get; set; }
    public string FileType { get; set; }
}

public sealed record AttachmentContentDto
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }

    public string FileName { get; set; }
    public int Size { get; set; }
    public string FileType { get; set; }
    public byte[] Content { get; set; }
}
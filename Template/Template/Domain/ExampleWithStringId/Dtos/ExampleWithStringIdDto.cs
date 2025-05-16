namespace Template.Domain.ExampleWithStringId.Dtos;

public sealed record ExampleWithStringIdDto
{
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}
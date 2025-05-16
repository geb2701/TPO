namespace Template.Domain.ExampleWithIntId.Dtos;

public sealed record ExampleWithIntIdDto
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public bool IsDeleted { get; set; }
    public string Name { get; set; }
}
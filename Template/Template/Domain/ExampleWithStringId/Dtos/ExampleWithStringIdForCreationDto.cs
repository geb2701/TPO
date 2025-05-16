namespace Template.Domain.ExampleWithStringId.Dtos;

public sealed record ExampleWithStringIdForCreationDto
{
    public string Name { get; set; }
    public string Code { get; set; }
}
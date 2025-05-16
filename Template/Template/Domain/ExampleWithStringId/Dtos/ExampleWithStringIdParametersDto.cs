using Template.Resources.QueryKitUtilities;

namespace Template.Domain.ExampleWithStringId.Dtos;

public sealed class ExampleWithStringIdParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
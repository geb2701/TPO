using Template.Resources.QueryKitUtilities;

namespace Template.Domain.ExampleWithIntId.Dtos;

public sealed class ExampleWithIntIdParametersDto : BasePaginationParameters
{
    public string? Filters { get; set; }
    public string? SortOrder { get; set; }
}
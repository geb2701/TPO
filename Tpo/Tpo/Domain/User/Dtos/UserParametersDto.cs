using Tpo.Resources.QueryKitUtilities;

namespace Tpo.Domain.User.Dtos
{
    public class UserParametersDto : BasePaginationParameters
    {
        public string? Filters { get; set; }
        public string? SortOrder { get; set; }
    }
}

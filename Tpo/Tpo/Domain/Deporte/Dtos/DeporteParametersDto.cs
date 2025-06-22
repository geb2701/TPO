using Tpo.Resources.QueryKitUtilities;

namespace Tpo.Domain.Deporte.Dtos
{
    public class DeporteParametersDto : BasePaginationParameters
    {
        public string? Filters { get; set; }
        public string? SortOrder { get; set; }
    }
}

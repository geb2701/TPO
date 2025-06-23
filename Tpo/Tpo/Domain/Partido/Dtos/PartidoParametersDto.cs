using Tpo.Resources.QueryKitUtilities;

namespace Tpo.Domain.Partido.Dtos
{
    public class PartidoParametersDto : BasePaginationParameters
    {
        public string? Filters { get; set; }
        public string? SortOrder { get; set; }
    }
}

using Tpo.Resources.QueryKitUtilities;

namespace Tpo.Domain.Usuario.Dtos
{
    public class UsuarioParametersDto : BasePaginationParameters
    {
        public string? Filters { get; set; }
        public string? SortOrder { get; set; }
    }
}

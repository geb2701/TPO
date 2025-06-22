using Tpo.Domain.UsuarioDeporte.Dtos;
using Tpo.Domain.UsuarioDeporte.Models;

namespace Tpo.Domain.UsuarioDeporte.Mappings
{
    public static class UsuarioDeporteMapper
    {
        public static UsuarioDeporteForCreation ToUsuarioDeporteForCreation(this UsuarioDeporteForCreationDto dto)
        {
            return new UsuarioDeporteForCreation
            {
                UsuarioId = dto.UsuarioId,
                DeporteId = dto.DeporteId,
                Nivel = dto.Nivel
            };
        }

        public static UsuarioDeporteForUpdate ToUsuarioDeporteForUpdate(this UsuarioDeporteForUpdateDto dto)
        {
            return new UsuarioDeporteForUpdate
            {
                DeporteId = dto.DeporteId,
                Nivel = dto.Nivel
            };
        }
    }
}

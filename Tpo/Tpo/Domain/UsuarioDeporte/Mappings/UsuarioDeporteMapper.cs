using Riok.Mapperly.Abstractions;
using Tpo.Domain.UsuarioDeporte.Dtos;
using Tpo.Domain.UsuarioDeporte.Models;

namespace Tpo.Domain.UsuarioDeporte.Mappings
{
    [Mapper]
    [UseStaticMapper(typeof(UsuarioDeporteMapper))]
    public static partial class UsuarioDeporteMapper
    {
        [MapperIgnoreSource(nameof(UsuarioDeporteForCreationDto.DeporteId))]
        public static partial UsuarioDeporteForCreation ToUsuarioDeporteForCreation(
            this UsuarioDeporteForCreationDto dto, Usuario.Usuario usuario, Deporte.Deporte deporte);

        public static partial UsuarioDeporteForUpdate ToUsuarioDeporteForUpdate(
            this UsuarioDeporteForUpdateDto dto);

        [MapProperty([nameof(UsuarioDeporte.Usuario), nameof(Usuario.Usuario.Alias)], nameof(UsuarioDeporteDto.Alias))]
        [MapProperty([nameof(UsuarioDeporte.Deporte), nameof(Deporte.Deporte.Nombre)], nameof(UsuarioDeporteDto.DeporteNombre))]
        public static partial UsuarioDeporteDto ToUsuarioDeporteDto(this UsuarioDeporte entity);

        [UserMapping]
        private static EnumResponse ToEnum(NivelHabilidad x) => x.ToEnumResponse();
    }
}

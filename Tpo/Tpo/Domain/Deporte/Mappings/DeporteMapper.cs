using Riok.Mapperly.Abstractions;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Domain.Deporte.Models;

namespace Tpo.Domain.Deporte.Mappings;

[Mapper]
[UseStaticMapper(typeof(UsuarioDeporteMapper))]
public static partial class UsuarioDeporteMapper
{
    public static partial UsuarioDeporteForCreation ToDeporteForCreation(
        this UsuarioDeporteForCreationDto dto);

    public static partial UsuarioDeporteForUpdate ToDeporteForUpdate(
        this UsuarioDeporteForUpdateDto dto);

    public static partial UsuarioDeporteDto ToDeporteDto(this Deporte entity);

    public static partial IQueryable<UsuarioDeporteDto>
        ToDeporteDtoQueryable(this IQueryable<Deporte> entity);
}
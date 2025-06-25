using Riok.Mapperly.Abstractions;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Domain.Deporte.Models;
using Tpo.Domain.Partido.Dtos;

namespace Tpo.Domain.Deporte.Mappings;

[Mapper]
[UseStaticMapper(typeof(DeporteMapper))]
public static partial class DeporteMapper
{
    public static partial DeporteForCreation ToDeporteForCreation(
        this DeporteForCreationDto dto);

    public static partial DeporteForUpdate ToDeporteForUpdate(
        this DeporteForUpdateDto dto);

    [MapperIgnoreSource(nameof(Deporte.UsuariosDeportes))]
    public static partial DeporteDto ToDeporteDto(this Deporte entity);

    public static partial IQueryable<DeporteDto>
        ToDeporteDtoQueryable(this IQueryable<Deporte> entity);
}
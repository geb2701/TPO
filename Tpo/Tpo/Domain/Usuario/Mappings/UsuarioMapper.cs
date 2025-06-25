using Riok.Mapperly.Abstractions;
using Tpo.Domain.Usuario.Dtos;
using Tpo.Domain.Usuario.Models;

namespace Tpo.Domain.Usuario.Mappings;

[Mapper]
[UseStaticMapper(typeof(UsuarioMapper))]
public static partial class UsuarioMapper
{
    public static partial UsuarioForCreation ToUsuarioForCreation(
        this UsuarioForCreationDto dto);

    public static partial UsuarioForUpdate ToUsuarioForUpdate(
        this UsuarioForUpdateDto dto);

    public static partial UsuarioDto ToUsuarioDto(this Usuario entity);

    public static partial IQueryable<UsuarioDto>
        ToUsuarioDtoQueryable(this IQueryable<Usuario> entity);

    [UserMapping]
    private static EnumResponse ToEnum(NivelHabilidad x) => x.ToEnumResponse();
    [UserMapping]
    private static EnumResponse ToEnum(TipoNotificacion x) => x.ToEnumResponse();
    [UserMapping]
    private static HabilidadDto ToHabilidadDto(UsuarioDeporte.UsuarioDeporte x) => new()
    {
        DeporteId = x.Deporte.Id,
        DeporteNombre = x.Deporte.Nombre,
        Nivel = x.Nivel.ToEnumResponse()
    };
    [UserMapping]
    private static ParticipanteDto ToParticipanteDto(Jugador.Jugador x) => new()
    {
        DeporteId = x.Partido.Deporte.Id,
        DeporteNombre = x.Partido.Deporte.Nombre,
        PartidoId = x.Partido.Id,
        PartidoEstado = x.Partido.Estado.Nombre
    };
}
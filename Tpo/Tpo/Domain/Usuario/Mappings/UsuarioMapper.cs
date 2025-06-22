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
}
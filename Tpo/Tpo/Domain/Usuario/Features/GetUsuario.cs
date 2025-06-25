using MediatR;
using Microsoft.EntityFrameworkCore;
using Tpo.Domain.Usuario.Dtos;
using Tpo.Domain.Usuario.Mappings;
using Tpo.Domain.Usuario.Services;

namespace Tpo.Domain.Usuario.Features;
public static class GetUsuario
{
    public sealed record Query(int Id) : IRequest<UsuarioDto>;

    public sealed class Handler(IUsuarioRepository repository) : IRequestHandler<Query, UsuarioDto>
    {
        public async Task<UsuarioDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetById(
                request.Id,
                true,
                cancellationToken,
                q => q.Include(u => u.Habilidades).ThenInclude(h => h.Deporte)
            );

            return entity?.ToUsuarioDto();
        }
    }
}
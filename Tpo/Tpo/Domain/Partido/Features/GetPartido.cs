using MediatR;
using Microsoft.EntityFrameworkCore;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Mappings;
using Tpo.Domain.Partido.Services;

namespace Tpo.Domain.Partido.Features;
public static class GetPartido
{
    public sealed record Query(int Id) : IRequest<PartidoDto>;

    public sealed class Handler(IPartidoRepository repository) : IRequestHandler<Query, PartidoDto>
    {
        public async Task<PartidoDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetById(
                request.Id,
                true,
                cancellationToken,
                q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario).Include(p => p.Deporte)
            );

            return entity?.ToPartidoDto();
        }
    }
}
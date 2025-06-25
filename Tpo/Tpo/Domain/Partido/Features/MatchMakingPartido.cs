using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Databases;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Mappings;
using Tpo.Domain.Partido.Services;
using Tpo.Domain.Usuario.Services;

namespace Tpo.Domain.Partido.Features
{
    public class MatchMakingPartido
    {
        public sealed record Query(int Id) : IRequest<MatchMakingDto>;

        public sealed class Handler(IPartidoRepository repository, IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork) : IRequestHandler<Query, MatchMakingDto>
        {
            public async Task<MatchMakingDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var partido = await repository.GetById(
                    request.Id,
                    true,
                    cancellationToken,
                    q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario).Include(p => p.Deporte)
                );

                var candidatos = usuarioRepository.Query(includes: q => q.Include(x => x.Habilidades).ThenInclude(x => x.Deporte).Include(x => x.Participante).ThenInclude(x => x.Partido)).ToList();

                partido.Emparejamiento(candidatos);

                await unitOfWork.CommitChanges(cancellationToken);

                var matchMakingDto = new MatchMakingDto
                {
                    Mensaje = "Jugadores seleccionados e inscritos correctamente.",
                    Jugadores = [.. partido.Jugadores.Select(j => PartidoMapper.ToJugadorDto(j))]
                };

                if (!partido.TieneJugadoresSuficientes())
                {
                    matchMakingDto.Mensaje = "No se han podido seleccionar suficientes jugadores para el partido.";
                }


                return matchMakingDto;
            }
        }
    }
}

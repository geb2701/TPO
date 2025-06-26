using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Databases;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Mappings;
using Tpo.Domain.Partido.Services;
using Tpo.Domain.Usuario.Services;

namespace Tpo.Domain.Partido.Features;
public static class AddUserToPartido
{
    public sealed record Command(int PartidoId, int UsuarioId) : IRequest<PartidoDto>;

    public sealed class Handler(IPartidoRepository repository, IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork) : IRequestHandler<Command, PartidoDto>
    {
        public async Task<PartidoDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetById(
                request.PartidoId,
                true,
                cancellationToken,
                q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario).Include(p => p.Deporte)
            );

            var usuario = await usuarioRepository.GetById(
                request.UsuarioId,
                true,
                cancellationToken,
                q => q.Include(u => u.Habilidades).ThenInclude(h => h.Deporte).Include(u => u.Participante).ThenInclude(p => p.Partido).ThenInclude(p => p.Deporte)
            );

            entity?.AgregarJugador(usuario);

            await unitOfWork.CommitChanges(cancellationToken);

            return entity?.ToPartidoDto();
        }
    }
}
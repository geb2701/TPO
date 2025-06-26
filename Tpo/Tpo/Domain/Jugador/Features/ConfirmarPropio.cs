using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Databases;
using Tpo.Domain.Partido.Services;
using Tpo.Exceptions;
using Tpo.Services;

namespace Tpo.Domain.Jugador.Features;

public static class ConfirmarPropio
{
    public sealed record Command(int PartidoId) : IRequest;

    public sealed class Handler(ICurrentUsuarioService currentUser, IPartidoRepository partidoRepository, IUnitOfWork unitOfWork) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var partido = await partidoRepository.GetById(
                request.PartidoId,
                true,
                cancellationToken,
                q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario).Include(p => p.Deporte)
            );

            var jugador = partido.Jugadores.FirstOrDefault(j => j.UsuarioId == currentUser.GetUsuarioId()) ?? throw new NotFoundException("Jugador no encontrado en el partido.");
            if (!jugador.Confirmar())
                throw new ValidationException("Ya confirmado o el estado no lo permite.");

            await unitOfWork.CommitChanges(cancellationToken);
        }
    }
}

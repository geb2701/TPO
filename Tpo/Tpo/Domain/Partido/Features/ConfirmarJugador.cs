using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Databases;
using Tpo.Domain.Jugador.Services;
using Tpo.Exceptions;

namespace Tpo.Domain.Partido.Features;

public static class ConfirmarJugador
{
    public sealed record Command(int JugadorId) : IRequest;

    public sealed class Handler(IJugadorRepository jugadorRepository, IUnitOfWork unitOfWork) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var jugador = await jugadorRepository.GetById(request.JugadorId, true, cancellationToken, q => q.Include(j => j.Usuario).Include(j => j.Partido).ThenInclude(x => x.Jugadores));

            if (!jugador.Confirmar())
                throw new ValidationException("Ya confirmado o el estado no lo permite.");

            await unitOfWork.CommitChanges(cancellationToken);
        }
    }
}

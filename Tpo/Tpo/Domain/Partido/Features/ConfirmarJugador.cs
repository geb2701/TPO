using MediatR;
using Microsoft.EntityFrameworkCore;
using Tpo.Databases;
using Tpo.Exceptions;

namespace Tpo.Domain.Partido.Features;

public static class ConfirmarJugador
{
    public sealed record Command(int JugadorId) : IRequest;

    public sealed class Handler(TpoDbContext db) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var jugador = await db.Jugador
                .Include(j => j.Partido)
                .FirstOrDefaultAsync(j => j.Id == request.JugadorId, cancellationToken);

            if (jugador is null)
                throw new NotFoundException("Jugador no encontrado.");

            if (!jugador.Confirmar())
                throw new ValidationException("Ya confirmado o el estado no lo permite.");

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}

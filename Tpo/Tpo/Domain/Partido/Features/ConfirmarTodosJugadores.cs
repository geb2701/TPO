using MediatR;
using Microsoft.EntityFrameworkCore;
using Tpo.Databases;

namespace Tpo.Domain.Partido.Features;

public static class ConfirmarTodosJugadores
{
    public sealed record Command(int PartidoId) : IRequest;

    public sealed class Handler(TpoDbContext db) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var jugadores = await db.Jugador
                .Include(j => j.Partido)
                .Where(j => j.PartidoId == request.PartidoId)
                .ToListAsync(cancellationToken);

            foreach (var jugador in jugadores)
                jugador.Confirmar();

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}

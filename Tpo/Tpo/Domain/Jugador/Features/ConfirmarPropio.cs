using MediatR;
using Microsoft.EntityFrameworkCore;
using Tpo.Databases;
using Tpo.Exceptions;
using Tpo.Services;

namespace Tpo.Domain.Jugador.Features;

public static class ConfirmarPropio
{
    public sealed record Command(int PartidoId) : IRequest;

    public sealed class Handler(TpoDbContext db, ICurrentUsuarioService currentUser) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var usuarioId = currentUser.GetUsuarioId();

            var jugador = await db.Jugador
                .Include(j => j.Partido)
                .FirstOrDefaultAsync(j => j.UsuarioId == usuarioId && j.PartidoId == request.PartidoId, cancellationToken);

            if (jugador is null)
                throw new NotFoundException("No estás anotado en este partido.");

            if (!jugador.Confirmar())
                throw new ValidationException("Ya confirmado o el estado no lo permite.");

            await db.SaveChangesAsync(cancellationToken);
        }
    }
}

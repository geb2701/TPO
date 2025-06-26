using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Databases;
using Tpo.Domain.Partido.Services;

namespace Tpo.Domain.Partido.Features;

public static class ConfirmarTodosJugadores
{
    public sealed record Command(int PartidoId) : IRequest;

    public sealed class Handler(IPartidoRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var partido = await repository.GetById(
                request.PartidoId,
                true,
                cancellationToken,
                q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario).Include(p => p.Deporte)
            );

            foreach (var jugador in partido.Jugadores)
                jugador.Confirmar();

            await unitOfWork.CommitChanges(cancellationToken);
        }
    }
}

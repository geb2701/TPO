using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Databases;
using Tpo.Domain.Partido.Services;
using Tpo.Services;

namespace Tpo.Domain.Partido.Features;
public static class CancelPartido
{
    public sealed record Command(int Id) : IRequest;

    public sealed class Handler(IPartidoRepository repository, ICurrentUsuarioService currentUsuarioService, IUnitOfWork unitOfWork) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetById(
                request.Id,
                true,
                cancellationToken,
                q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario).Include(p => p.Deporte)
            );

            if (entity.CreatedBy != null && entity.CreatedBy != currentUsuarioService.GetAlias())
                throw new InvalidOperationException("Solo el creador del partido puede cancelarlo.");

            if (entity.Estado is FinalizadoState || entity.Estado is CanceladoState)
                throw new InvalidOperationException("El partido ya ha finalizado o ya ha sido cancelado.");

            entity.Cancelar();

            await unitOfWork.CommitChanges(cancellationToken);
        }
    }
}
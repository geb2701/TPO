using Microsoft.EntityFrameworkCore;
using SharedKernel.Databases;
using Tpo.Domain.Partido;
using Tpo.Domain.Partido.Services;

namespace Tpo.Workers
{
    public class PartidoWorker(IPartidoRepository partidoRepository, IUnitOfWork unitOfWork) : BackgroundService
    {
        private readonly TimeSpan _intervalo = TimeSpan.FromMinutes(1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var ahora = DateTime.Now;
                var partidosConfimados = partidoRepository
                    .Query(x => x.Estado is ConfirmadoState && x.FechaHora <= ahora, stoppingToken, true,
                        q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario))
                    .ToList();

                foreach (var partido in partidosConfimados)
                {
                    partido.AvanzarEstado();
                }

                var partidosCompletados = partidoRepository
                    .Query(x => x.Estado is ConfirmadoState && x.FechaHora + x.Duracion <= ahora, stoppingToken, true,
                        q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario))
                    .ToList();

                foreach (var partido in partidosCompletados)
                {
                    partido.AvanzarEstado();
                }

                await unitOfWork.CommitChanges(stoppingToken);

                await Task.Delay(_intervalo, stoppingToken);
            }
        }
    }
}

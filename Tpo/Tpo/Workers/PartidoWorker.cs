using Microsoft.EntityFrameworkCore;
using SharedKernel.Databases;
using Tpo.Domain.Partido;
using Tpo.Domain.Partido.Services;

namespace Tpo.Workers
{
    public class PartidoWorker(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly TimeSpan _intervalo = TimeSpan.FromMinutes(1);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceProvider.CreateScope();
            var partidoRepository = scope.ServiceProvider.GetRequiredService<IPartidoRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            while (!stoppingToken.IsCancellationRequested)
            {
                var ahora = DateTime.Now;
                var partidosConfimados = partidoRepository
                    .Query(x => x.FechaHora < ahora, true, stoppingToken,
                        q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario))
                    .AsEnumerable()
                    .Where(x => x.Estado is ConfirmadoState)
                    .ToList();

                foreach (var partido in partidosConfimados)
                {
                    partido.AvanzarEstado();
                }

                var partidosCompletados = partidoRepository
                    .Query(x => x.FechaHora + x.Duracion < ahora, true, stoppingToken,
                        q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario))
                    .AsEnumerable()
                    .Where(x => x.Estado is EnJuegoState)
                    .ToList();

                foreach (var partido in partidosCompletados)
                {
                    partido.AvanzarEstado();
                }

                var partidosACancelar = partidoRepository
                    .Query(x => x.FechaHora + x.Duracion < ahora, true, stoppingToken,
                        q => q.Include(p => p.Jugadores).ThenInclude(j => j.Usuario))
                    .AsEnumerable()
                    .Where(x => x.Estado is ConfirmadoState || x.Estado is PartidoArmadoState || x.Estado is NecesitamosJugadoresState)
                    .ToList();

                foreach (var partido in partidosACancelar)
                {
                    partido.Cancelar();
                }

                await unitOfWork.SystemCommitChanges(stoppingToken);

                await Task.Delay(_intervalo, stoppingToken);
            }
        }
    }
}

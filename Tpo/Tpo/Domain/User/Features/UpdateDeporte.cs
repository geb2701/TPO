using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Databases;

namespace Tpo.Domain.Deporte.Features
{
    public class UpdateDeporte
    {
        private readonly TpoDbContext _context;

        public UpdateDeporte(TpoDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(int id, UpdateDeporteDto dto)
        {
            var deporte = await _context.Deportes.FirstOrDefaultAsync(d => d.Id == id);
            if (deporte == null)
            {
                throw new KeyNotFoundException("Deporte no encontrado.");
            }

            deporte.Update(dto.Nombre, dto.Dificultad);
            await _context.SaveChangesAsync();
        }
    }
}

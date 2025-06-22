using System.Threading.Tasks;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Databases;
using Tpo.Domain.Deporte;

namespace Tpo.Domain.Deporte.Features
{
    public class AddDeporte
    {
        private readonly TpoDbContext _context;

        public AddDeporte(TpoDbContext context)
        {
            _context = context;
        }

        public async Task<int> ExecuteAsync(AddDeporteDto dto)
        {
            var deporte = Deporte.Create(dto.Nombre, dto.Dificultad);
            _context.Deportes.Add(deporte);
            await _context.SaveChangesAsync();
            return deporte.Id;
        }
    }
}

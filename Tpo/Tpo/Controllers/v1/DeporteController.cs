using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Domain.Deporte.Features;

namespace Tpo.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeporteController : ControllerBase
    {
        private readonly AddDeporte _addDeporte;
        private readonly UpdateDeporte _updateDeporte;

        public DeporteController(AddDeporte addDeporte, UpdateDeporte updateDeporte)
        {
            _addDeporte = addDeporte;
            _updateDeporte = updateDeporte;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] AddDeporteDto dto)
        {
            var id = await _addDeporte.ExecuteAsync(dto);
            return Ok(new { Id = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDeporteDto dto)
        {
            await _updateDeporte.ExecuteAsync(id, dto);
            return NoContent();
        }
    }
}


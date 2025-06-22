using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tpo.Attributes;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Domain.Deporte.Features;
using Tpo.Domain.Usuario.Features;

namespace Tpo.Controllers.v1
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class DeporteController(ILogger<DeporteController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<DeporteController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost(Name = "DeporteAdd")]
        public async Task<IActionResult> DeporteAdd([FromBody] UsuarioDeporteForCreationDto dto)
        {
            var command = new AddUsuarioDeporte.Command(dto);
            var queryResponse = await _mediator.Send(command);
            return Ok(queryResponse);
        }
    }
}


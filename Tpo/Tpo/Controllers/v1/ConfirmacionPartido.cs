using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tpo.Domain.Jugador.Features;
using Tpo.Domain.Partido.Features;

namespace Tpo.Controllers.v1
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class ConfirmacionPartido(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        
        [HttpPost("confirmar-propio/{partidoId}")]
        public async Task<IActionResult> ConfirmarPropio([FromRoute] int partidoId)
        {
            await _mediator.Send(new ConfirmarPropio.Command(partidoId));
            return Ok("Jugador confirmado");
        }

        [HttpPost("confirmar-otro/{jugadorId}")]
        public async Task<IActionResult> ConfirmarOtro([FromRoute] int jugadorId)
        {
            await _mediator.Send(new ConfirmarJugador.Command(jugadorId));
            return Ok("Jugador confirmado");
        }

        [HttpPost("confirmar-todos/{partidoId}")]
        public async Task<IActionResult> ConfirmarTodos([FromRoute] int partidoId)
        {
            await _mediator.Send(new ConfirmarTodosJugadores.Command(partidoId));
            return Ok("Todos los jugadores confirmados");
        }
    }
}

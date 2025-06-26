using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Tpo.Attributes;
using Tpo.Domain.Jugador.Features;
using Tpo.Domain.Partido.Dtos;
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

        [HttpPost("{idPartido}/Usuario/{idUsuario}", Name = "AddUserToPartido")]
        public async Task<IActionResult> AddUserToPartido([FromRoute] int idPartido, [FromRoute] int idUsuario)
        {
            var command = new AddUserToPartido.Command(idPartido, idUsuario);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("{id}/MatchMaking", Name = "MatchMaking")]
        public async Task<IActionResult> MatchMaking([FromRoute] int id)
        {
            var command = new MatchMakingPartido.Command(id);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("{id}/Cancelar", Name = "CancelarPatido")]
        public async Task<IActionResult> CancelarPartido([FromRoute] int id)
        {
            var command = new CancelPartido.Command(id);
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet("PartidosDisponibles", Name = "GetPartidosDisponibles")]
        public async Task<IActionResult> GetPartidosDisponibles([FromQuery] PartidoParametersDto parametersDto)
        {
            var query = new GetPartidoList.Query(parametersDto);
            var queryResponse = await _mediator.Send(query);

            var paginationMetadata = new
            {
                totalCount = queryResponse.TotalCount,
                pageSize = queryResponse.PageSize,
                currentPageSize = queryResponse.CurrentPageSize,
                currentStartIndex = queryResponse.CurrentStartIndex,
                currentEndIndex = queryResponse.CurrentEndIndex,
                pageNumber = queryResponse.PageNumber,
                totalPages = queryResponse.TotalPages,
                hasPrevious = queryResponse.HasPrevious,
                hasNext = queryResponse.HasNext
            };

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationMetadata);

            return Ok(queryResponse);
        }

        [HttpPost("Confirmar/{partidoId}", Name = "ConfirmarPropio")]
        public async Task<IActionResult> ConfirmarPropio([FromRoute] int partidoId)
        {
            await _mediator.Send(new ConfirmarPropio.Command(partidoId));
            return Ok("Jugador confirmado");
        }

        [HttpPost("ConfirmarOtro/{jugadorId}", Name = "ConfirmarOtro")]
        public async Task<IActionResult> ConfirmarOtro([FromRoute] int jugadorId)
        {
            await _mediator.Send(new ConfirmarJugador.Command(jugadorId));
            return Ok("Jugador confirmado");
        }

        [HttpPost("ConfirmarTodos/{partidoId}", Name = "ConfirmarTodos")]
        public async Task<IActionResult> ConfirmarTodos([FromRoute] int partidoId)
        {
            await _mediator.Send(new ConfirmarTodosJugadores.Command(partidoId));
            return Ok("Todos los jugadores confirmados");
        }
    }
}

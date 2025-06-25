using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Tpo.Attributes;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Features;

namespace Tpo.Controllers.v1
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class PartidoController(ILogger<PartidoController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<PartidoController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost("Historial", Name = "PartidoAddHistorial")]
        public async Task<IActionResult> PartidoAddHistorial([FromBody] PartidoHistorialForCreationDto dto)
        {
            var command = new AddPartidoHistorial.Command(dto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("Nivel", Name = "PartidoAddNivel")]
        public async Task<IActionResult> PartidoAddNivel([FromBody] PartidoNivelForCreationDto dto)
        {
            var command = new AddPartidoNivel.Command(dto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("Ubicacion", Name = "PartidoAddUbicacion")]
        public async Task<IActionResult> PartidoAddUbicacion([FromBody] PartidoUbicacionForCreationDto dto)
        {
            var command = new AddPartidoUbicacion.Command(dto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        [HttpGet(Name = "GetPartidos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] PartidoParametersDto parametersDto)
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

    }
}


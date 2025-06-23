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

        [HttpPost(Name = "PartidoAdd")]
        public async Task<IActionResult> PartidoAdd([FromBody] PartidoHistorialForCreationDto dto)
        {
            var command = new AddPartido.Command(dto);
            var queryResponse = await _mediator.Send(command);
            return Ok(queryResponse);
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


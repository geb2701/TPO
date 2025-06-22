using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Tpo.Attributes;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Domain.Deporte.Features;

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
        public async Task<IActionResult> DeporteAdd([FromBody] DeporteForCreationDto dto)
        {
            var command = new AddDeporte.Command(dto);
            var queryResponse = await _mediator.Send(command);
            return Ok(queryResponse);
        }


        [HttpGet(Name = "GetDeportes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] DeporteParametersDto parametersDto)
        {
            var query = new GetDeportesList.Query(parametersDto);
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


using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tpo.Domain.Usuario.Features;
using Tpo.Domain.Usuario.Dtos;
using System.Text.Json;

namespace Tpo.Controllers.v1
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UsuarioController(ILogger<UsuarioController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost("Register", Name = "UsuarioRegister")]
        public async Task<ActionResult<UsuarioDto>> Register([FromBody] UsuarioForCreationDto dto)
        {
            var command = new AddUsuario.Command(dto);
            var commandResponse = await _mediator.Send(command);
            return Ok(commandResponse);
        }

        [HttpPost("Login", Name = "UsuarioLogin")]
        public async Task<ActionResult<string>> Get([FromBody] UsuarioLoginDto userLoginDto)
        {
            var command = new LoginUsuario.Command(userLoginDto);
            var commandResponse = await _mediator.Send(command);
            return Ok(commandResponse);
        }

        [HttpGet(Name = "GetUsuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] UsuarioParametersDto parametersDto)
        {
            var query = new GetUsuariosList.Query(parametersDto);
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


        [HttpPut("{id}", Name = "Update")]
        public async Task<ActionResult<UsuarioDto>> Update([FromRoute] int id, [FromBody] UsuarioForUpdateDto dto)
        {
            var command = new UpdateUsuario.Command(id, dto);
            await _mediator.Send(command);
            return Ok();
        }
    }
}

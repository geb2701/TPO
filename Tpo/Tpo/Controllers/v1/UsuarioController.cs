using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Tpo.Domain.Usuario.Dtos;
using Tpo.Domain.Usuario.Features;

namespace Tpo.Controllers.v1
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UsuarioController(ILogger<UsuarioController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpPost("Register", Name = "UsuarioRegister")]
        public async Task<ActionResult<UsuarioDto>> Register([FromBody] UsuarioForCreationDto dto)
        {
            var command = new AddUsuario.Command(dto);
            var commandResponse = await mediator.Send(command);
            return Ok(commandResponse);
        }

        [HttpPost("Login", Name = "UsuarioLogin")]
        public async Task<ActionResult<string>> Get([FromBody] UsuarioLoginDto userLoginDto)
        {
            var command = new LoginUsuario.Command(userLoginDto);
            var commandResponse = await mediator.Send(command);
            return Ok(commandResponse);
        }

        [HttpGet(Name = "GetUsuarios")]
        public async Task<IActionResult> GetList([FromQuery] UsuarioParametersDto parametersDto)
        {
            var query = new GetUsuariosList.Query(parametersDto);
            var queryResponse = await mediator.Send(query);

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

        [HttpGet("{id}", Name = "GetUsuario")]
        public async Task<IActionResult> GetId([FromRoute] int id)
        {
            var query = new GetUsuario.Query(id);
            var queryResponse = await mediator.Send(query);

            return Ok(queryResponse);
        }


        [HttpPut("{id}", Name = "UpdateUsuario")]
        [Authorize]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UsuarioForUpdateDto dto)
        {
            var command = new UpdateUsuario.Command(id, dto);
            await mediator.Send(command);
            return Ok();
        }
    }
}

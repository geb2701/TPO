using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Template.Domain.ExampleWithIntId.Features;
using Template.Domain.User.Dtos;
using Template.Domain.User.Features;

namespace Template.Controllers.v1
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController(ILogger<ExampleWithIntIdController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<ExampleWithIntIdController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("Register", Name = "Register")]
        public async Task<ActionResult<UserDto>> Register(UserForCreationDto dto)
        {
            var query = new AddUser.Command(dto);
            var queryResponse = await _mediator.Send(query);
            return Ok(queryResponse);
        }

        [HttpGet("Login", Name = "Login")]
        public async Task<ActionResult<UserDto>> Get(int id)//cambiar parametros
        {
            var query = new GetExampleWithIntId.Query(id);
            var queryResponse = await _mediator.Send(query);
            return Ok(queryResponse);
        }

        [HttpPut("Edit", Name = "Edit")]
        public async Task<ActionResult<UserDto>> Edit(int id)
        {
            var query = new GetExampleWithIntId.Query(id);
            var queryResponse = await _mediator.Send(query);
            return Ok(queryResponse);
        }
    }
}

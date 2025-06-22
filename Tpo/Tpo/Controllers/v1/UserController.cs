using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tpo.Domain.User.Features;
using Tpo.Domain.User.Dtos;

namespace Tpo.Controllers.v1
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController(ILogger<UserController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost("Register", Name = "UserRegister")]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserForCreationDto dto)
        {
            var query = new AddUser.Command(dto);
            var queryResponse = await _mediator.Send(query);
            return Ok(queryResponse);
        }

        [HttpPost("Login", Name = "UserLogin")]
        public async Task<ActionResult<string>> Get([FromBody] UserLoginDto userLoginDto)
        {
            var query = new LoginUser.Command(userLoginDto);
            var queryResponse = await _mediator.Send(query);
            return Ok(queryResponse);
        }
        /*
        [HttpPut("Edit", Name = "UserEdit")]
        public async Task<ActionResult<UserDto>> Edit(int id)
        {
            var query = new GetUser.Query(id);
            var queryResponse = await _mediator.Send(query);
            return Ok(queryResponse);
        }*/
    }
}

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Template.Domain.User.Features;
using Template.Domain.User.Dtos;
using Template.Domain.User.Features;

namespace Template.Controllers.v1
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DeporteController(ILogger<UserController> logger, IMediator mediator) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            var query = new GetUser.Query(id);
            var queryResponse = await _mediator.Send(query);
            return Ok(queryResponse);
        }

        [HttpPut("Edit", Name = "Edit")]
        public async Task<ActionResult<UserDto>> Edit(int id)
        {
            var query = new GetUser.Query(id);
            var queryResponse = await _mediator.Send(query);
            return Ok(queryResponse);
        }

        //Usuario añadir deporte con habulidad - el usuario tiene un tipo de notificacion
        //Deporte crear, editar, obtener
        //Partido crear, obtener, busquedada, inscribirse a partido, confirmar, cancelar

        //sistema de notificaciones

        //Factory Crear modelos
        //Observer: escuchar eventos de partido
        //State: estados del patido
        //Adptarter: envio de mails
        //Stategy eleccion de partidos
    }
}

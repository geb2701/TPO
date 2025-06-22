using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tpo.Domain.User.Features;
using Tpo.Domain.User.Dtos;
using Tpo.Attributes;
using Tpo.Services;

namespace Tpo.Controllers.v1
{
    [ApiController]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Authorize]
    public class DeporteController(ILogger<UserController> logger, IMediator mediator, ICurrentUserService currentUserService) : ControllerBase
    {
        private readonly ILogger<UserController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost("Register", Name = "Register")]
        public async Task<ActionResult<UserDto>> Register(UserForCreationDto dto)
        {
            var userId = currentUserService.GetUser();
            var query = new AddUser.Command(dto);
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

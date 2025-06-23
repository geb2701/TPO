using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tpo.Attributes;
using Tpo.Domain.UsuarioDeporte.Dtos;
using Tpo.Domain.UsuarioDeporte.Features;

namespace Tpo.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsuarioDeporteController(IMediator mediator) : ControllerBase
{
    [HttpPost(Name = "AddUsuarioDeporte")]
    public async Task<IActionResult> Create([FromBody] UsuarioDeporteForCreationDto dto)
    {
        var id = await mediator.Send(new AddUsuarioDeporte.Command(dto));
        return Ok(new { Id = id });
    }

    [HttpPut("{id}", Name = "UpdateUsuarioDeporte")]
    [Authorize]
    public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UsuarioDeporteForUpdateDto dto)
    {
        var command = new UpdateUsuarioDeporte.Command(id, dto);
        await mediator.Send(command);
        return Ok();
    }
}

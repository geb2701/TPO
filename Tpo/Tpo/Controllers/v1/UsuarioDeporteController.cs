using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tpo.Domain.UsuarioDeporte.Dtos;
using Tpo.Domain.UsuarioDeporte.Features;

namespace Tpo.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class UsuarioDeporteController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuarioDeporteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UsuarioDeporteForCreationDto dto)
    {
        var id = await _mediator.Send(new AddUsuarioDeporte.Command(dto));
        return Ok(new { Id = id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioDeporteForUpdateDto dto)
    {
        await _mediator.Send(new UpdateUsuarioDeporte.Command(id, dto));
        return NoContent();
    }
}

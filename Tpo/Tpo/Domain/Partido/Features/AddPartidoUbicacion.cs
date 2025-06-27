using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Databases;
using Tpo.Domain.Deporte.Services;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Mappings;
using Tpo.Domain.Partido.Services;
using Tpo.Domain.Usuario.Services;
using Tpo.Exceptions;
using Tpo.Extensions.Application;
using Tpo.Services;

namespace Tpo.Domain.Partido.Features;

public class AddPartidoUbicacion
{
    public sealed record Command(PartidoUbicacionForCreationDto Dto) : IRequest<PartidoDto>;

    public class AddPartidoValidator : AbstractValidator<PartidoUbicacionForCreationDto>
    {
        public AddPartidoValidator()
        {
            RuleFor(x => x.FechaHora)
                .GreaterThan(DateTime.Now)
                .WithMessage("La fecha y hora debe ser mayor a la actual.");

            RuleFor(x => x.Ubicacion)
                .NotEmpty().WithMessage("La ubicación es obligatoria.")
                .Length(3, 100).WithMessage("La ubicación debe tener entre 3 y 100 caracteres.");

            RuleFor(x => x.CantidadParticipantes)
                .GreaterThan(0).WithMessage("La cantidad de participantes debe ser mayor a 0.");

            RuleFor(x => x.DuracionMinutos)
                .GreaterThan(0).WithMessage("La duración debe ser mayor a 0 minutos.");

            RuleFor(x => x.DeporteId)
                .GreaterThan(0).WithMessage("El ID de deporte debe ser mayor a 0.");
        }
    }

    public sealed class Handler(IPartidoRepository repository, IUnitOfWork unitOfWork, IDeporteRepository deporteRepository, IUsuarioRepository usuarioRepository, ICurrentUsuarioService currentUsuarioService,
                AddPartidoValidator validator) : IRequestHandler<Command, PartidoDto>
    {
        public async Task<PartidoDto> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request.Dto);

            var deporte = await deporteRepository.GetByIdOrDefault(request.Dto.DeporteId, cancellationToken: cancellationToken)
                ?? throw new NotFoundException($"Deporte con ID {request.Dto.DeporteId} no encontrado.");
            var model = request.Dto.ToPartidoForCreation(deporte);
            var entity = Partido.Create(model);

            var usuario = await usuarioRepository.GetById(
                currentUsuarioService.GetUsuarioId(),
                true,
                cancellationToken,
                x => x.Include(u => u.Habilidades)
                      .Include(u => u.Participante)
                          .ThenInclude(p => p.Partido)
                              .ThenInclude(pa => pa.Deporte)
            );

            entity.AgregarJugador(usuario);

            await repository.Add(entity, cancellationToken);
            await unitOfWork.CommitChanges(cancellationToken);

            return entity.ToPartidoDto();
        }
    }
}
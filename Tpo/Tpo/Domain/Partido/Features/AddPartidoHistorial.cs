using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Deporte.Services;
using Tpo.Domain.Partido.Dtos;
using Tpo.Domain.Partido.Mappings;
using Tpo.Domain.Partido.Services;
using Tpo.Exceptions;
using Tpo.Extensions.Application;

namespace Tpo.Domain.Partido.Features;

public class AddPartidoHistorial
{
    public sealed record Command(PartidoHistorialForCreationDto Dto) : IRequest<PartidoDto>;

    public class AddPartidoValidator : AbstractValidator<PartidoHistorialForCreationDto>
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

            RuleFor(x => x.PartidosMinimosJugados)
                .GreaterThanOrEqualTo(0).WithMessage("Los partidos mínimos jugados no pueden ser negativos.");
        }
    }

    public sealed class Handler(IPartidoRepository repository, IUnitOfWork unitOfWork, IDeporteRepository deporteRepository,
                AddPartidoValidator validator) : IRequestHandler<Command, PartidoDto>
    {
        public async Task<PartidoDto> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request.Dto);

            var deporte = await deporteRepository.GetByIdOrDefault(request.Dto.DeporteId, cancellationToken: cancellationToken)
                ?? throw new NotFoundException($"Deporte con ID {request.Dto.DeporteId} no encontrado.");
            var model = request.Dto.ToPartidoForCreation(deporte);
            var entity = Partido.Create(model);

            await repository.Add(entity, cancellationToken);
            await unitOfWork.CommitChanges(cancellationToken);

            return entity.ToPartidoDto();
        }
    }
}
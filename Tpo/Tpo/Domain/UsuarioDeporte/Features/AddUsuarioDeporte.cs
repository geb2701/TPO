using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Deporte.Services;
using Tpo.Domain.Usuario.Services;
using Tpo.Domain.UsuarioDeporte.Dtos;
using Tpo.Domain.UsuarioDeporte.Mappings;
using Tpo.Domain.UsuarioDeporte.Services;
using Tpo.Extensions.Application;
using Tpo.Services;

namespace Tpo.Domain.UsuarioDeporte.Features;

public class AddUsuarioDeporte
{
    public sealed record Command(UsuarioDeporteForCreationDto Dto) : IRequest<UsuarioDeporteDto>;

    public class AddUsuarioDeporteValidator : AbstractValidator<Command>
    {
        public AddUsuarioDeporteValidator()
        {
            RuleFor(x => x.Dto.DeporteId)
                .GreaterThan(0).WithMessage("El deporte es obligatorio.");

            RuleFor(x => x.Dto.Nivel)
                .IsInEnum().WithMessage("El nivel de habilidad no es válido.");
        }
    }

    public sealed class Handler(IUsuarioDeporteRepository repository, IDeporteRepository deporteRepository, IUsuarioRepository usuarioRepository,
        IUnitOfWork unitOfWork, AddUsuarioDeporteValidator validator,
        ICurrentUsuarioService currentUsuarioService) : IRequestHandler<Command, UsuarioDeporteDto>
    {
        public async Task<UsuarioDeporteDto> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var deporte = await deporteRepository.GetById(request.Dto.DeporteId, withTracking: true, cancellationToken: cancellationToken) ?? throw new ArgumentException($"El deporte con ID {request.Dto.DeporteId} no existe.");
            var usuario = await usuarioRepository.GetById(currentUsuarioService.GetUsuarioId(), withTracking: true, cancellationToken: cancellationToken) ?? throw new ArgumentException("El usuario no existe.");
            var model = request.Dto.ToUsuarioDeporteForCreation(usuario, deporte);

            var entity = UsuarioDeporte.Create(model);

            var registers = repository.Query(includes: [x => x.Deporte, x => x.Usuario]);

            if (registers.Any(x => x.Usuario.Id == entity.Usuario.Id && x.Deporte.Id == entity.Deporte.Id))
            {
                throw new ArgumentException($"El usuario ya tiene el deporte {deporte.Nombre} registrado.");
            }

            if (registers.Any(x => x.Usuario.Id == entity.Usuario.Id && x.Favorito && entity.Favorito))
            {
                throw new ArgumentException($"El usuario ya tiene otro deporte como favorito.");
            }

            await repository.Add(entity, cancellationToken);
            await unitOfWork.CommitChanges(cancellationToken);
            return entity.ToUsuarioDeporteDto();
        }
    }
}

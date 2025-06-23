using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Domain.Deporte.Mappings;
using Tpo.Domain.Deporte.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.Deporte.Features;

public class AddDeporte
{
    public sealed record Command(DeporteForCreationDto Dto) : IRequest<DeporteDto>;

    public class AddDeporteValidator : AbstractValidator<Command>
    {
        public AddDeporteValidator()
        {
            RuleFor(x => x.Dto.Nombre).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler(IDeporteRepository repository, IUnitOfWork unitOfWork,
            AddDeporteValidator validator) : IRequestHandler<Command, DeporteDto>
    {
        public async Task<DeporteDto> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var model = request.Dto.ToDeporteForCreation();
            var entity = Deporte.Create(model);

            if (repository.Query().Any(x => x.Nombre == entity.Nombre))
                throw new ApplicationException("El deporte ya existe.");

            await repository.Add(entity, cancellationToken);
            await unitOfWork.CommitChanges(cancellationToken);

            return entity.ToDeporteDto();
        }
    }
}
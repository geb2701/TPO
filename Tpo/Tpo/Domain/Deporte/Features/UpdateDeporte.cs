using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Domain.Deporte.Mappings;
using Tpo.Domain.Deporte.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.Deporte.Features;

public class UpdateDeporte
{
    public sealed record Command(int Id, DeporteForUpdateDto Dto) : IRequest;

    public class UpdateDeporteValidator : AbstractValidator<Command>
    {
        public UpdateDeporteValidator()
        {
            RuleFor(x => x.Dto.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(3, 50).WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler(IDeporteRepository repository, IUnitOfWork unitOfWork, UpdateDeporteValidator validator) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var entity = await repository.GetById(request.Id, true, cancellationToken);
            var model = request.Dto.ToDeporteForUpdate();

            entity.Update(model);

            repository.Update(entity);
            await unitOfWork.CommitChanges(cancellationToken);
        }
    }
}

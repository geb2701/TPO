using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.Deporte.Mappings;
using Tpo.Domain.Deporte.Dtos;
using Tpo.Domain.Deporte.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.Deporte.Features;

public class UpdateDeporte
{
    public sealed record Command(int Id, UpdateDeporteDto Dto) : IRequest;

    public class UpdateDeporteValidator : AbstractValidator<Command>
    {
        public UpdateDeporteValidator()
        {
            RuleFor(x => x.Dto.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(3, 50).WithMessage("El nombre debe tener entre 3 y 50 caracteres.");

            RuleFor(x => x.Dto.Dificultad)
                .InclusiveBetween(1, 10).WithMessage("La dificultad debe estar entre 1 y 10.");
        }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IDeporteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UpdateDeporteValidator _validator;

        public Handler(IDeporteRepository repository, IUnitOfWork unitOfWork,
            UpdateDeporteValidator validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrowValidationException(request);

            var entity = await _repository.GetById(request.Id, cancellationToken);
            var model = request.Dto.ToDeporteForUpdate();

            entity.Update(model);

            _repository.Update(entity);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}

using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Template.Domain.ExampleWithStringId.Dtos;
using Template.Domain.ExampleWithStringId.Mappings;
using Template.Domain.ExampleWithStringId.Services;
using Template.Extensions.Application;

namespace Template.Domain.ExampleWithStringId.Features;

public class UpdateExampleWithStringId
{
    public sealed record Command(string Code, ExampleWithStringIdForUpdateDto Dto) : IRequest;

    public class UpdateExampleWithStringIdValidator : AbstractValidator<Command>
    {
        public UpdateExampleWithStringIdValidator()
        {
            RuleFor(x => x.Dto.Name).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IExampleWithStringIdRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UpdateExampleWithStringIdValidator _validator;

        public Handler(IExampleWithStringIdRepository repository, IUnitOfWork unitOfWork,
            UpdateExampleWithStringIdValidator validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrowValidationException(request);

            var entity =
                await _repository.GetById(request.Code, cancellationToken);
            var model = request.Dto.ToExampleWithStringIdForUpdate();

            entity.Update(model);

            _repository.Update(entity);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}
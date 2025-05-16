using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Template.Domain.ExampleWithStringId.Dtos;
using Template.Domain.ExampleWithStringId.Mappings;
using Template.Domain.ExampleWithStringId.Services;
using Template.Extensions.Application;

namespace Template.Domain.ExampleWithStringId.Features;

public class AddExampleWithStringId
{
    public sealed record Command(ExampleWithStringIdForCreationDto Dto) : IRequest<ExampleWithStringIdDto>;

    public class AddExampleValidator : AbstractValidator<Command>
    {
        public AddExampleValidator()
        {
            RuleFor(x => x.Dto.Code).Length(2, 50)
                .WithMessage("El código debe tener entre 2 y 50 caracteres.");
            RuleFor(x => x.Dto.Name).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler : IRequestHandler<Command, ExampleWithStringIdDto>
    {
        private readonly IExampleWithStringIdRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AddExampleValidator _validator;

        public Handler(IExampleWithStringIdRepository repository, IUnitOfWork unitOfWork,
            AddExampleValidator validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<ExampleWithStringIdDto> Handle(Command request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrowValidationException(request);

            var model = request.Dto.ToExampleWithStringIdForCreation();
            var entity = ExampleWithStringId.Create(model);

            if (_repository.Query().Any(x => x.Code == entity.Code))
                throw new ApplicationException("El código proporcionado ya corresponde a otro Example.");

            await _repository.Add(entity, cancellationToken);
            await _unitOfWork.CommitChanges(cancellationToken);

            return entity.ToExampleWithStringIdDto();
        }
    }
}
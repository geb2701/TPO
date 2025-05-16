using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Template.Domain.ExampleWithIntId.Dtos;
using Template.Domain.ExampleWithIntId.Mappings;
using Template.Domain.ExampleWithIntId.Services;
using Template.Extensions.Application;

namespace Template.Domain.ExampleWithIntId.Features;

public class AddExampleWithIntId
{
    public sealed record Command(ExampleWithIntIdForCreationDto Dto) : IRequest<ExampleWithIntIdDto>;

    public class AddExampleValidator : AbstractValidator<Command>
    {
        public AddExampleValidator()
        {
            RuleFor(x => x.Dto.Name).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler : IRequestHandler<Command, ExampleWithIntIdDto>
    {
        private readonly IExampleWithIntIdRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AddExampleValidator _validator;

        public Handler(IExampleWithIntIdRepository repository, IUnitOfWork unitOfWork,
            AddExampleValidator validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<ExampleWithIntIdDto> Handle(Command request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrowValidationException(request);

            var model = request.Dto.ToExampleWithIntIdForCreation();
            var entity = ExampleWithIntId.Create(model);

            if (_repository.Query().Any(x => x.Id == entity.Id))
                throw new ApplicationException("El código proporcionado ya corresponde a otro Example.");

            await _repository.Add(entity, cancellationToken);
            await _unitOfWork.CommitChanges(cancellationToken);

            return entity.ToExampleWithIntIdDto();
        }
    }
}
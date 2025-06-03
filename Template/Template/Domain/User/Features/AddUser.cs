using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Template.Domain.ExampleWithIntId.Mappings;
using Template.Domain.User.Dtos;
using Template.Domain.User.Services;
using Template.Extensions.Application;

namespace Template.Domain.User.Features;

public class AddUser
{
    public sealed record Command(UserForCreationDto Dto) : IRequest<UserDto>;

    public class AddExampleValidator : AbstractValidator<Command>
    {
        public AddExampleValidator()
        {
            RuleFor(x => x.Dto.Name).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler(IExampleWithIntIdRepository repository, IUnitOfWork unitOfWork,
            AddExampleValidator validator) : IRequestHandler<Command, UserDto>
    {
        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var model = request.Dto.ToExampleWithIntIdForCreation();
            var entity = User.Create(model);

            if (repository.Query().Any(x => x.Id == entity.Id))
                throw new ApplicationException("El código proporcionado ya corresponde a otro Example.");

            await repository.Add(entity, cancellationToken);
            await unitOfWork.CommitChanges(cancellationToken);

            return entity.ToExampleWithIntIdDto();
        }
    }
}
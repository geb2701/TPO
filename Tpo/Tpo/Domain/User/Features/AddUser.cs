using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Tpo.Domain.User.Dtos;
using Tpo.Domain.User.Mappings;
using Tpo.Domain.User.Services;
using Tpo.Extensions.Application;

namespace Tpo.Domain.User.Features;

public class AddUser
{
    public sealed record Command(UserForCreationDto Dto) : IRequest<UserDto>;

    public class AddUserValidator : AbstractValidator<Command>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Dto.Name).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler(IUserRepository repository, IUnitOfWork unitOfWork,
            AddUserValidator validator) : IRequestHandler<Command, UserDto>
    {
        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            validator.ValidateAndThrowValidationException(request);

            var model = request.Dto.ToUserForCreation();
            var entity = User.Create(model);

            if (repository.Query().Any(x => x.Name == entity.Name))
                throw new ApplicationException("El Name ya existe.");

            await repository.Add(entity, cancellationToken);
            await unitOfWork.CommitChanges(cancellationToken);

            return entity.ToUserDto();
        }
    }
}
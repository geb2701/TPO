using FluentValidation;
using MediatR;
using SharedKernel.Databases;
using Template.Domain.User.Mappings;
using Template.Domain.User.Dtos;
using Template.Domain.User.Mappings;
using Template.Domain.User.Services;
using Template.Extensions.Application;

namespace Template.Domain.User.Features;

public class UpdateUser
{
    public sealed record Command(int Id, UserForUpdateDto Dto) : IRequest;

    public class UpdateUserValidator : AbstractValidator<Command>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Dto.Name).Length(3, 50)
                .WithMessage("El nombre debe tener entre 3 y 50 caracteres.");
        }
    }

    public sealed class Handler : IRequestHandler<Command>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UpdateUserValidator _validator;

        public Handler(IUserRepository repository, IUnitOfWork unitOfWork,
            UpdateUserValidator validator)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrowValidationException(request);

            var entity =
                await _repository.GetById(request.Id, cancellationToken);
            var model = request.Dto.ToUserForUpdate();

            entity.Update(model);

            _repository.Update(entity);
            await _unitOfWork.CommitChanges(cancellationToken);
        }
    }
}
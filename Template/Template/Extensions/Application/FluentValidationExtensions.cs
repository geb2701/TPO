using FluentValidation;
using ValidationException = Template.Exceptions.ValidationException;

namespace Template.Extensions.Application;

public static class FluentValidationExtensions
{
    public static void ValidateAndThrowValidationException<T>(this IValidator<T> validator, T instance)
    {
        var res = validator.Validate(instance);

        if (!res.IsValid) throw new ValidationException(res.Errors);
    }
}
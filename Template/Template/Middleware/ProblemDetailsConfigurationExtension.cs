// source: https://github.com/jasontaylordev/CleanArchitecture/blob/main/src/WebUI/Filters/ApiExceptionFilterAttribute.cs

using Hellang.Middleware.ProblemDetails;
using Template.Exceptions;
using ValidationException = FluentValidation.ValidationException;

namespace Template.Middleware;

using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

public static class ProblemDetailsConfigurationExtension
{
    public static void ConfigureProblemDetails(ProblemDetailsOptions options)
    {
        options.MapFluentValidationException();
        options.MapValidationException();
        options.MapToStatusCode<ForbiddenAccessException>(StatusCodes.Status401Unauthorized);
        options.MapToStatusCode<NoRolesAssignedException>(StatusCodes.Status403Forbidden);
        options.MapToStatusCode<NotFoundException>(StatusCodes.Status404NotFound);

        // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
        // This is useful if you have upstream middleware that needs to do additional handling of exceptions.
        // options.Rethrow<NotSupportedException>();

        options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
        options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

        // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
        // This is useful if you have upstream middleware that  needs to do additional handling of exceptions.
        // options.Rethrow<NotSupportedException>();

        // You can configure the middleware to ignore any exceptions to the specified type.
        // This is useful if you have upstream middleware that  needs to do additional handling of exceptions.
        // Note that unlike Rethrow, additional information will not be added to the exception.
        // options.Ignore<DivideByZeroException>();

        // Because exceptions are handled polymorphic, this will act as a "catch all" mapping, which is why it's added last.
        // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
        options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
    }

    private static void MapFluentValidationException(this ProblemDetailsOptions options)
    {
        options.Map<ValidationException>((ctx, ex) =>
        {
            var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            var errors = ex.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(x => x.ErrorMessage).ToArray());

            var error = factory.CreateValidationProblemDetails(ctx, errors);

            // workaround: add exception message to error.
            error.Title = ex.Message;
            return error;
        });
    }

    private static void MapValidationException(this ProblemDetailsOptions options)
    {
        options.Map<Exceptions.ValidationException>((ctx, ex) =>
        {
            var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            // var errors = ex.Errors
            //     .GroupBy(x => x.Key, x => x.Value)
            //     .ToDictionary(
            //         x => x.Key,
            //         x => x.Select(x => x.ToString())
            //             .ToArray()
            //         );

            //var errors = ex.Errors;

            var error = factory.CreateValidationProblemDetails(ctx, ex.Errors);

            // workaround: add exception message to error.
            error.Title = ex.Message;
            return error;
        });
    }
}
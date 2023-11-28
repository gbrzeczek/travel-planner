using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using TravelPlanner.Api.Common.Exceptions;

namespace TravelPlanner.Api.Common;

public class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(
            "Error Message: {ExceptionMessage}, Time of occurrence {Time}",
            exception.Message, DateTime.UtcNow);

        var statusCode = GetStatusCode(exception);

        var response = new
        {
            Title = GetTitle(exception),
            Errors = GetErrors(exception),
            Status = statusCode
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);

        return true;
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            ApplicationValidationException => StatusCodes.Status422UnprocessableEntity,
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

    private static string GetTitle(Exception exception) =>
        exception switch
        {
            ValidationException => "Validation Error",
            ApplicationValidationException => "Validation Error",
            NotFoundException => "Not Found",
            UnauthorizedException => "Unauthorized",
            _ => "Server Error"
        };
    
    private record ValidationError(string Key, string ErrorMessage);

    private static IEnumerable<ValidationError> GetErrors(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => validationException.Errors
                .Select(x => new ValidationError(x.PropertyName, x.ErrorMessage)),
            ApplicationValidationException => new[] { new ValidationError("Error", exception.Message) },
            NotFoundException => new[] { new ValidationError("Error", exception.Message) },
            UnauthorizedException => Enumerable.Empty<ValidationError>(),
            _ => Enumerable.Empty<ValidationError>()
        };
    }
}
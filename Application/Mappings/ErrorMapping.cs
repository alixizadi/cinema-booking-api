using CinemaApp.Domain.Abstractions;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace CinemaApp.Application.Mappings;

public static class ErrorMapping
{
    public static Error ToError(this ValidationResult validationResult)
    {
        return new Error($"Error.Validation.{validationResult.Errors.FirstOrDefault()!.PropertyName}", validationResult.Errors.FirstOrDefault()!.ErrorMessage);
    }
}
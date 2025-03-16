using CinemaApp.Domain.Abstractions;

namespace Account.Application.Exceptions;

public sealed record ValidationError(string PropertyName, string ErrorName)
{
    public Error ToError() => new Error($"Error.Validation.{PropertyName}", ErrorName);
};

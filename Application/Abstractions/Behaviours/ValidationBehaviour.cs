using Account.Application.Exceptions;
using CinemaApp.Application.Abstractions.Messaging;
using FluentValidation;
using MediatR;

namespace CinemaApp.Application.Abstractions.Behaviours;

public class ValidationBehaviour <TRequest, TResponse> 
: IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if(!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(v => v.Validate(context))
            .Where(result => result.Errors.Any())
            .SelectMany(v => v.Errors)
            .Select(f => new ValidationError(
                f.PropertyName,
                f.ErrorMessage
                ))
            .ToList();
        if (validationErrors.Any())
        {
            // throw new global::Account.Application.Exceptions.ValidationException(validationErrors);
        }

        return await next();
    }
}
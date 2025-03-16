using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Application.Mappings;
using CinemaApp.Application.Validations;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;
using FluentValidation;

namespace CinemaApp.Application.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<RegisterUserCommand> _validator;
    public RegisterUserCommandHandler(IUserRepository userRepository, IValidator<RegisterUserCommand> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Result.Failure<RegisterUserResponse>(validationResult.ToError());;
        }
        
        if (await _userRepository.ExistsAsync(request.Email))
            return Result.Failure<RegisterUserResponse>(UserErrors.EmailAlreadyExists);

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.CreateUser(request.Email, request.Password, request.Name, request.IsAdmin);

        await _userRepository.AddAsync(user);

        return Result.Success(new RegisterUserResponse(user.Id));
    }
}
using CinemaApp.Application.Abstractions.Auth;
using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Application.Mappings;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;
using FluentValidation;

namespace CinemaApp.Application.Commands.LoginUserCommand;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _jwtTokenService;
    private readonly IValidator<LoginUserCommand> _validator;

    public LoginUserCommandHandler(IUserRepository userRepository, IAuthService jwtTokenService, IValidator<LoginUserCommand> validator)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _validator = validator;
    }

    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return Result.Failure<LoginUserResponse>(validationResult.ToError());;
        }
        
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result.Failure<LoginUserResponse>(UserErrors.InvalidCredentials);

        var token = await _jwtTokenService.AuthenticateAsync(user.Email, user.PasswordHash);

        if (string.IsNullOrWhiteSpace(token))
        {
            return Result.Failure<LoginUserResponse>(UserErrors.InvalidCredentials);
        }

        return Result.Success(new LoginUserResponse(token));
    }
}
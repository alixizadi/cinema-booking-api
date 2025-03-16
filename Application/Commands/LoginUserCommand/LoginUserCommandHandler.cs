using CinemaApp.Application.Abstractions.Auth;
using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.LoginUserCommand;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _jwtTokenService;

    public LoginUserCommandHandler(IUserRepository userRepository, IAuthService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
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
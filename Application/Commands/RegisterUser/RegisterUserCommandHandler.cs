using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsAsync(request.Email))
            return Result.Failure<RegisterUserResponse>(UserErrors.EmailAlreadyExists);

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = User.CreateUser(request.Email, request.Password, request.Name, request.IsAdmin);

        await _userRepository.AddAsync(user);

        return Result.Success(new RegisterUserResponse(user.Id));
    }
}
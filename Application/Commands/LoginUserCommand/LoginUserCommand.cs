using CinemaApp.Application.Abstractions.Messaging;

namespace CinemaApp.Application.Commands.LoginUserCommand;

public sealed class LoginUserCommand : ICommand<LoginUserResponse>
{
    public string Email { get; }
    public string Password { get; }

    public LoginUserCommand(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
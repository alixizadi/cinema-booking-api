using CinemaApp.Application.Abstractions.Messaging;

namespace CinemaApp.Application.Commands.RegisterUser;


public sealed class RegisterUserCommand : ICommand<RegisterUserResponse>
{
    public string Email { get; }
    public string Password { get; }
    public string? Name { get; }
    
    public bool IsAdmin { get; }

    public RegisterUserCommand(string email, string password, string? name, bool isAdmin = false)
    {
        Email = email;
        Password = password;
        Name = name;
        IsAdmin = isAdmin;
    }
}
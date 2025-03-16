

namespace CinemaApp.Application.Abstractions.Auth;
public interface IAuthService
{
    Task<string?> AuthenticateAsync(string email, string password);
}

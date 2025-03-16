using CinemaApp.Domain.Abstractions;

namespace CinemaApp.Domain.Entities;
public class User : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    
    public string? Name { get; private set; }
    public bool IsAdmin { get; private set; }

    private User()
    {
    }


    public static User CreateUser(string email, string password, string? name = null, bool isAdmin = false)
    {
        User user = new User
        {
            Email = email,
            PasswordHash = HashPassword(password),
            Name = name,
            IsAdmin = isAdmin
        };
        return user;
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
    }
}

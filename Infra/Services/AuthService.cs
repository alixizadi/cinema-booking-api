

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CinemaApp.Application.Abstractions.Auth;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace CinemaApp.Infra.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<string?> AuthenticateAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || user.PasswordHash != password) // TODO: Hash & Compare Password
            return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var secretKey = _configuration["JwtSettings:Secret"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim("UserId", user.Id.ToString()),
            new Claim("IsAdmin", user.IsAdmin.ToString())
        };
        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:ExpirationMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsAsync(string email);
    Task AddUserAsync(User user);
}
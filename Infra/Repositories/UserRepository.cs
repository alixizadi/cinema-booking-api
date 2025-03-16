using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Repositories;
using CinemaApp.Infra.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infra.Repositories;
    public class UserRepository : Repository<User>, IUserRepository
    {

        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }

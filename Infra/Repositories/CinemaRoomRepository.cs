
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Repositories;
using CinemaApp.Infra.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
namespace CinemaApp.Infra.Repositories;

public class CinemaRoomRepository : Repository<CinemaRoom>, ICinemaRoomRepository
{
    public CinemaRoomRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<CinemaRoom>> GetRoomsWithSchedulesAsync()
    {
        return await _context.CinemaRooms
            .Include(c => c.Schedules)
            .ToListAsync();
    }

    public async Task<bool> RoomWithNameExists(string name)
    {
        return await _context.CinemaRooms.AnyAsync(c => c.Name.ToLower() == name.ToLower());
    }
}

using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Repositories;
using CinemaApp.Infra.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infra.Repositories;

public class MovieScheduleRepository : Repository<MovieSchedule>, IMovieScheduleRepository
{
    public MovieScheduleRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<MovieSchedule>> GetSchedulesByRoomAsync(Guid cinemaRoomId, DateTime after)
    {
        return await _context.MovieSchedules
            .Where(ms => ms.CinemaRoomId == cinemaRoomId && ms.StartTime > after)
            .Include(ms=> ms.Movie)
            .ToListAsync();
    }

    public async Task<IEnumerable<MovieSchedule>> GetSchedulesByMovieAsync(Guid movieId)
    {
        return await _context.MovieSchedules
            .Where(ms => ms.Movie.Id == movieId)
            .ToListAsync();
    }

    public async Task<MovieSchedule?> GetScheduleWithMovie(Guid id)
    {
        return await _dbSet.Include(i=>i.Movie).FirstOrDefaultAsync(i=>i.Id == id);
    }

    public async Task<List<Booking>> GetBookingsByScheduleAsync(Guid movieScheduleId)
    {
        return await _context.Bookings
            .Where(b=>b.MovieScheduleId == movieScheduleId).ToListAsync();
    }

    public async Task<bool> IsSeatBookedAsync(Guid movieScheduleId, int row, int number)
    {
        return await _context.Bookings.AnyAsync(s =>
            s.MovieScheduleId == movieScheduleId &&
            s.Seat.Row == row && s.Seat.Number == number);
    }

    public async Task BookSeatAsync(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsRoomReserved(Guid roomId, DateTime startTime, DateTime endTime)
    {
        return await _context.MovieSchedules.AnyAsync(m => m.CinemaRoomId == roomId 
                && ( (m.StartTime > startTime && m.StartTime < endTime) || ((m.EndTime > startTime && m.EndTime < endTime))));
    }
}
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Domain.Repositories;

public interface IMovieScheduleRepository : IRepository<MovieSchedule>
{
    Task<IEnumerable<MovieSchedule>> GetSchedulesByRoomAsync(Guid cinemaRoomId, DateTime afterDateTime);
    Task<IEnumerable<MovieSchedule>> GetSchedulesByMovieAsync(Guid movieId);
    
    Task<MovieSchedule?> GetScheduleWithMovie(Guid movieScheduleId);
    
    Task<List<Booking>> GetBookingsByScheduleAsync(Guid movieScheduleId);
    Task<bool> IsSeatBookedAsync(Guid movieScheduleId, int row, int col);
    Task BookSeatAsync(Booking booking);

    Task<bool> IsRoomReserved(Guid roomId, DateTime startTime, DateTime endTime);
}

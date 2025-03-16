using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.ValueObjects;

namespace CinemaApp.Domain.Entities;

public class MovieSchedule : BaseEntity
{
    public Guid CinemaRoomId { get; private set; }
    public Movie Movie { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    
    private readonly List<Booking> _bookedSeats = new();

    private MovieSchedule()
    {
    }

    public IReadOnlyCollection<Booking> BookedSeats => _bookedSeats.AsReadOnly();

    public MovieSchedule(Guid cinemaRoomId, Movie movie, DateTime startTime)
    {
        CinemaRoomId = cinemaRoomId;
        Movie = movie;
        StartTime = startTime;
    }

    public void BookSeat(Booking booking)
    {
        if (_bookedSeats.Contains(booking))
            throw new InvalidOperationException("Seat is already booked for this movie.");

        booking.Seat.Book();
        _bookedSeats.Add(booking);
    }

    public static MovieSchedule Create(Guid roomId, Movie movie, DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
        {
            throw new Exception("Movie end time should be after start time");
        }

        var movieSchedule = new MovieSchedule()
        {
            CinemaRoomId = roomId,
            Movie = movie,
            StartTime = startTime,
            EndTime = endTime
        };

        return movieSchedule;
    }

    public bool UpdateTimes( DateTime startTime, DateTime endTime)
    {
        if (endTime > startTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            return true;
        }

        return false;
    }
}

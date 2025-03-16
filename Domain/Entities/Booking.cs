using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.ValueObjects;

namespace CinemaApp.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid MovieScheduleId { get; private set; }
    public Seat Seat { get; private set; }
    public Guid UserId { get; private set; }

    private Booking()
    {
        
    }

    public Booking(Guid movieScheduleId, Seat seat, Guid userId)
    {
        MovieScheduleId = movieScheduleId;
        Seat = seat;
        UserId = userId;
    }
}

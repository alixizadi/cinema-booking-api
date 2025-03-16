using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Queries.GetRoomScheduleSeats;

public sealed record RoomScheduleSeatsResponse(
    Guid ScheduleId,
    string RoomName,
    string MovieName,
    string MovieUrl,
    DateTime StartTime,
    DateTime EndTime,
    List<SeatResponse> Seats);

public sealed record SeatResponse(int Row, int Number, bool IsBooked);
using CinemaApp.Application.Abstractions.Messaging;

namespace CinemaApp.Application.Queries.GetRoomScheduleSeats;

public sealed class GetRoomScheduleSeatsQuery : IQuery<RoomScheduleSeatsResponse>
{
    public Guid RoomId { get; }
    public Guid ScheduleId { get; }

    public GetRoomScheduleSeatsQuery(Guid roomId, Guid scheduleId)
    {
        RoomId = roomId;
        ScheduleId = scheduleId;
    }
}
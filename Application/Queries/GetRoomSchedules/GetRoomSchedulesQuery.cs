using CinemaApp.Application.Abstractions.Messaging;

namespace CinemaApp.Application.Queries.GetRoomSchedules;

public sealed class GetRoomSchedulesQuery : IQuery<List<RoomScheduleResponse>>
{
    public Guid RoomId { get; }

    public GetRoomSchedulesQuery(Guid roomId)
    {
        RoomId = roomId;
    }
}
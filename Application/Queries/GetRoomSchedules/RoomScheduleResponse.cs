namespace CinemaApp.Application.Queries.GetRoomSchedules;

public sealed record RoomScheduleResponse
(

    Guid ScheduleId,
    Guid RoomId,
    string RoomName,
    Guid MovieId,
    string MovieTitle,
    string PosterUrl,
    DateTime StartTime,
    DateTime EndTime);
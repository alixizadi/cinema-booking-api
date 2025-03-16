using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Queries.GetRoomSchedules;

public sealed class GetRoomSchedulesQueryHandler : IQueryHandler<GetRoomSchedulesQuery, List<RoomScheduleResponse>>
{
    private readonly IMovieScheduleRepository _movieScheduleRepository;
    private readonly ICinemaRoomRepository _roomRepository;
    

    public GetRoomSchedulesQueryHandler(IMovieScheduleRepository movieScheduleRepository, ICinemaRoomRepository roomRepository)
    {
        _movieScheduleRepository = movieScheduleRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Result<List<RoomScheduleResponse>>> Handle(GetRoomSchedulesQuery request, CancellationToken cancellationToken)
    {

        var room = await _roomRepository.GetByIdAsync(request.RoomId);

        if (room is null)
        {
            return Result.Failure<List<RoomScheduleResponse>>(RoomErrors.RoomNotFound);
        }
        var schedules = await _movieScheduleRepository
            .GetSchedulesByRoomAsync(request.RoomId, DateTime.Now);

        if (!schedules.Any())
            return Result.Failure<List<RoomScheduleResponse>>(ScheduleErrors.NoSchedulesFound);
        
        
        var response = schedules
            .Select(s => new RoomScheduleResponse(s.Id , room.Id, room.Name, s.Movie.Id, s.Movie.Title, s.Movie.PosterUrl, s.StartTime, s.EndTime))
            .ToList();

        return Result.Success(response);
    }
}
using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.Admin.MovieSchedules;

public sealed record GetAllMovieSchedulesQuery(Guid RoomId) : IQuery<List<MovieScheduleDto>>;

public sealed record MovieScheduleDto(Guid MovieScheduleId, Guid CinemaRoomId, Guid MovieId, DateTime StartTime, DateTime EndTime)
{
}

public sealed class GetAllMovieSchedulesQueryHandler : IQueryHandler<GetAllMovieSchedulesQuery, List<MovieScheduleDto>>
{
    private readonly IMovieScheduleRepository _movieScheduleRepository;

    public GetAllMovieSchedulesQueryHandler(IMovieScheduleRepository movieScheduleRepository)
    {
        _movieScheduleRepository = movieScheduleRepository;
    }

    public async Task<Result<List<MovieScheduleDto>>> Handle(GetAllMovieSchedulesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var movieSchedules = await _movieScheduleRepository.GetSchedulesByRoomAsync(request.RoomId, DateTime.MinValue);
            var result = movieSchedules.Select(m => new MovieScheduleDto
            (m.Id, m.CinemaRoomId, m.Movie.Id, m.StartTime, m.EndTime)).ToList();

            return Result.Success(result);
        }
        catch (Exception e)
        {
            return Result.Failure<List<MovieScheduleDto>>(Error.FromException(e));
        }
    }
}
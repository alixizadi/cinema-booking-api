using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Queries.GetRoomScheduleSeats;

public sealed class GetRoomScheduleSeatsQueryHandler : IQueryHandler<GetRoomScheduleSeatsQuery, RoomScheduleSeatsResponse>
{
    private readonly ICinemaRoomRepository _cinemaRoomRepository;
    private readonly IMovieScheduleRepository _scheduleRepository;

    public GetRoomScheduleSeatsQueryHandler(ICinemaRoomRepository cinemaRoomRepository, IMovieScheduleRepository scheduleRepository)
    {
        _cinemaRoomRepository = cinemaRoomRepository;
        _scheduleRepository = scheduleRepository;
    }

    public async Task<Result<RoomScheduleSeatsResponse>> Handle(GetRoomScheduleSeatsQuery request, CancellationToken cancellationToken)
    {
        var room = await _cinemaRoomRepository.GetByIdAsync(request.RoomId);
        if (room is null)
            return Result.Failure<RoomScheduleSeatsResponse>(RoomErrors.RoomNotFound);
        var schedule = await _scheduleRepository.GetScheduleWithMovie(request.ScheduleId);

        if (schedule == null)
        {
            return Result.Failure<RoomScheduleSeatsResponse>(ScheduleErrors.ScheduleNotFound);
        }
        var bookings = await _scheduleRepository.GetBookingsByScheduleAsync(request.ScheduleId);

        var seats = room.Seats();
        var seatResponses = seats
            .Select(s => new SeatResponse(s.Row, s.Number, bookings.Any(b=> b.Seat.Row == s.Row && b.Seat.Number == s.Number)))
            .ToList();
        
        var response = new RoomScheduleSeatsResponse(
            schedule.Id,
            room.Name,
            schedule.Movie.Title,
            schedule.Movie.PosterUrl,
            schedule.StartTime,
            schedule.EndTime,
            seatResponses);
        return Result.Success(response);
    }
}
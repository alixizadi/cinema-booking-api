using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;
using CinemaApp.Domain.ValueObjects;

namespace CinemaApp.Application.Commands.BookSeatCommand;
public sealed class BookSeatCommandHandler : ICommandHandler<BookSeatCommand, List<Guid>>
{
    private readonly ICinemaRoomRepository _cinemaRoomRepository;
    private readonly IMovieScheduleRepository _movieScheduleRepository;
    private readonly IUserRepository _userRepository;

    public BookSeatCommandHandler(
        IMovieScheduleRepository movieScheduleRepository,
        IUserRepository userRepository, ICinemaRoomRepository cinemaRoomRepository)
    {
        _movieScheduleRepository = movieScheduleRepository;
        _userRepository = userRepository;
        _cinemaRoomRepository = cinemaRoomRepository;
    }

    public async Task<Result<List<Guid>>> Handle(BookSeatCommand request, CancellationToken cancellationToken)
    {
        
        // Validate MovieSchedule exists
        var schedule = await _movieScheduleRepository.GetByIdAsync(request.MovieScheduleId);
        if (schedule is null)
            return Result.Failure<List<Guid>>(ScheduleErrors.ScheduleNotFound);
        
        // Validate Room Exists
        var room = await _cinemaRoomRepository.GetByIdAsync(schedule.CinemaRoomId);

        if (room is null)
        {
            return Result.Failure<List<Guid>>(RoomErrors.RoomNotFound);
        }

        foreach (var requestedSeat in request.Seats)
        {
            if (requestedSeat.RowNumber <= 0 || requestedSeat.ColNumber <= 0 || room.Rows < requestedSeat.RowNumber || room.Columns < requestedSeat.ColNumber)
            {
                return Result.Failure<List<Guid>>(BookingErrors.SeatNotFound);
            }
        }


        foreach (var requestedSeat in request.Seats)
        {
            // Validate Seat exists
            var isSeatBooked = await _movieScheduleRepository.IsSeatBookedAsync(request.MovieScheduleId, requestedSeat.RowNumber, requestedSeat.ColNumber);
            if (isSeatBooked)
                return Result.Failure<List<Guid>>(BookingErrors.SeatAlreadyBooked);
        }



        // Validate User exists
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user is null)
            return Result.Failure<List<Guid>>(UserErrors.UserNotFound);

        try
        {
            // Create booking
            List<Guid> bookings = new();
            foreach (var requestedSeat in request.Seats)
            {
                var booking = new Booking(request.MovieScheduleId, new Seat(requestedSeat.RowNumber, requestedSeat.ColNumber), request.UserId);
                await _movieScheduleRepository.BookSeatAsync(booking);
                bookings.Add(booking.Id);
            }


            return Result.Success(bookings);
        }
        catch (Exception e)
        {
            return Result.Failure<List<Guid>>(Error.FromException(e));
        }
    }

}

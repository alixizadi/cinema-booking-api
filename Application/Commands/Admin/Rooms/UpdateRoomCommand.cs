using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.AdminCommands;

public sealed record UpdateRoomCommand(Guid RoomId, string NewName) : ICommand;

public sealed class UpdateRoomNameCommandHandler : ICommandHandler<UpdateRoomCommand>
{
    private readonly ICinemaRoomRepository _cinemaRoomRepository;

    public UpdateRoomNameCommandHandler(ICinemaRoomRepository roomRepository)
    {
        _cinemaRoomRepository = roomRepository;
    }


    public async Task<Result> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var room = await _cinemaRoomRepository.GetByIdAsync(request.RoomId);
            if (room is null)
                return Result.Failure(RoomErrors.RoomNotFound);
            var roomExists =await _cinemaRoomRepository.RoomWithNameExists(request.NewName);

            if (roomExists)
            {
                return Result.Failure<Guid>(RoomErrors.RoomWithNameExists);
            }
            room.UpdateDetails(request.NewName);
            await _cinemaRoomRepository.UpdateAsync(room);

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.FromException(e));
        }
    }
}
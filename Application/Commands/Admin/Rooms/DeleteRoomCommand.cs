using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.AdminCommands;

public sealed record DeleteRoomCommand(Guid RoomId) : ICommand;

public sealed class DeleteRoomCommandHandler : ICommandHandler<DeleteRoomCommand>
{
    private readonly ICinemaRoomRepository _cinemaRoomRepository;

    public DeleteRoomCommandHandler(ICinemaRoomRepository roomRepository)
    {
        _cinemaRoomRepository = roomRepository;
    }

    public async Task<Result> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var room = await _cinemaRoomRepository.GetByIdAsync(request.RoomId);
            if (room is null)
                return Result.Failure(RoomErrors.RoomNotFound);

            await _cinemaRoomRepository.DeleteAsync(room.Id);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.FromException(e));
        }
    }
}

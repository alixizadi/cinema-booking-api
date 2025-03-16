using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.AdminCommands;

public sealed record CreateRoomCommand(string Name) : ICommand<Guid>;

public sealed class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, Guid>
{
    private readonly ICinemaRoomRepository _cinemaRoomRepository;

    public CreateRoomCommandHandler(ICinemaRoomRepository roomRepository)
    {
        _cinemaRoomRepository = roomRepository;
    }

    public async Task<Result<Guid>> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var roomExists =await _cinemaRoomRepository.RoomWithNameExists(request.Name);

        if (roomExists)
        {
            return Result.Failure<Guid>(RoomErrors.RoomWithNameExists);
        }
        var room = new CinemaRoom(request.Name);
        try
        {
            await _cinemaRoomRepository.AddAsync(room);
            return Result.Success(room.Id);
        }
        catch (Exception e)
        {
            return Result.Failure<Guid>(Error.FromException(e));
        }
    }
}

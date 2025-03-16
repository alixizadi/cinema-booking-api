using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Application.Queries.GetAllRooms;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.AdminQueries;

public sealed record GetAllRoomsQuery : IQuery<List<CinemaRoomResponse>>;

public sealed class GetAllRoomsQueryHandler : IQueryHandler<GetAllRoomsQuery, List<CinemaRoomResponse>>
{
    private readonly ICinemaRoomRepository _roomRepository;

    public GetAllRoomsQueryHandler(ICinemaRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result<List<CinemaRoomResponse>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetAllAsync();
        return Result.Success(rooms.Select(r => new CinemaRoomResponse(r.Id, r.Name)).ToList());
    }
}
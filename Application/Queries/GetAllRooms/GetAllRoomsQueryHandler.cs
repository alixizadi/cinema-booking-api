using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Queries.GetAllRooms;

public sealed class GetAllRoomsQueryHandler : IQueryHandler<GetAllRoomsQuery, List<CinemaRoomResponse>>
{
    private readonly ICinemaRoomRepository _cinemaRoomRepository;

    public GetAllRoomsQueryHandler(ICinemaRoomRepository cinemaRoomRepository)
    {
        _cinemaRoomRepository = cinemaRoomRepository;
    }

    public async Task<Result<List<CinemaRoomResponse>>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _cinemaRoomRepository.GetAllAsync();

        var response = rooms.Select(r => new CinemaRoomResponse(r.Id, r.Name)).ToList();

        return Result.Success(response);
    }
}
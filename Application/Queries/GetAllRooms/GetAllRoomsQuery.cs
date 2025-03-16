using CinemaApp.Application.Abstractions.Messaging;

namespace CinemaApp.Application.Queries.GetAllRooms;

public sealed class GetAllRoomsQuery : IQuery<List<CinemaRoomResponse>>
{
}
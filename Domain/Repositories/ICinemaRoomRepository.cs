using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Domain.Repositories;

public interface ICinemaRoomRepository : IRepository<CinemaRoom>
{
    Task<IEnumerable<CinemaRoom>> GetRoomsWithSchedulesAsync();
    Task<bool> RoomWithNameExists(string name);
}

using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Domain.Repositories;

public interface IMovieRepository : IRepository<Movie>
{
    Task<IEnumerable<Movie>> GetMoviesByRoomAsync(Guid cinemaRoomId);
    Task<bool> MovieWithNameExists(string title);
}

using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Repositories;
using CinemaApp.Infra.DatabaseContexts;
using CinemaApp.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infra.Repositories;
public class MovieRepository : Repository<Movie>, IMovieRepository
{
    public MovieRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Movie>> GetMoviesByRoomAsync(Guid cinemaRoomId)
    {
        return await _context.MovieSchedules
            .Where(ms => ms.CinemaRoomId == cinemaRoomId)
            .Select(ms => ms.Movie)
            .Distinct()
            .ToListAsync();
    }

    public async Task<bool> MovieWithNameExists(string title)
    {
        return await _context.Movies.AnyAsync(c => c.Title.ToLower() == title.ToLower());
    }
}
using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.Admin.Movies;

public sealed record GetAllMoviesQuery() : IQuery<List<MovieResponse>>;
public sealed record MovieResponse(Guid Id, string Title, string PosterUrl);

public sealed class GetAllMoviesQueryHandler : IQueryHandler<GetAllMoviesQuery, List<MovieResponse>>
{
    private readonly IMovieRepository _movieRepository;

    public GetAllMoviesQueryHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<Result<List<MovieResponse>>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await _movieRepository.GetAllAsync();
        return Result.Success(movies.Select(m => new MovieResponse(m.Id, m.Title, m.PosterUrl)).ToList());
    }
}
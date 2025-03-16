using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.Admin.Movies;

public sealed record UpdateMovieCommand(Guid MovieId, string Title, string PosterUrl) : ICommand;

public sealed class UpdateMovieCommandHandler : ICommandHandler<UpdateMovieCommand>
{
    private readonly IMovieRepository _movieRepository;

    public UpdateMovieCommandHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<Result> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie is null)
            return Result.Failure(MovieErrors.MovieNotFound);

        var movieExists =await _movieRepository.MovieWithNameExists(request.Title);

        if (movieExists)
        {
            return Result.Failure<Guid>(MovieErrors.MovieWithNameExists);
        }

        movie.UpdateDetails(request.Title, request.PosterUrl);

        try
        {
            await _movieRepository.UpdateAsync(movie);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(Error.FromException(e));
        }
    }
}
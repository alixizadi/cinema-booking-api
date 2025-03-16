using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.Admin.Movies;

public sealed record DeleteMovieCommand(Guid MovieId) : ICommand;

public sealed class DeleteMovieCommandHandler : ICommandHandler<DeleteMovieCommand>
{
    private readonly IMovieRepository _movieRepository;

    public DeleteMovieCommandHandler(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<Result> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _movieRepository.GetByIdAsync(request.MovieId);
        if (movie is null)
            return Result.Failure(MovieErrors.MovieNotFound);

        try
        {
            await _movieRepository.DeleteAsync(movie.Id);
            return Result.Success();    
        }
        catch (Exception e)
        {
            return Result.Failure(Error.FromException(e));
        }
    }
}
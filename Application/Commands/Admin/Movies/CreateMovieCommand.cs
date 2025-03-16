using CinemaApp.Application.Abstractions.Messaging;
using CinemaApp.Domain.Abstractions;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Errors;
using CinemaApp.Domain.Repositories;

namespace CinemaApp.Application.Commands.Admin.Movies;


// public sealed record CreateMovieCommand(string Title, string PosterUrl) : ICommand<Guid>;
public sealed record CreateMovieCommand(string Title, IFormFile Poster) : ICommand<Guid>;


public sealed class CreateMovieCommandHandler : ICommandHandler<CreateMovieCommand, Guid>
{
    private readonly IMovieRepository _movieRepository;
    
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CreateMovieCommandHandler(IMovieRepository movieRepository, IWebHostEnvironment webHostEnvironment)
    {
        _movieRepository = movieRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<Result<Guid>> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movieExists =await _movieRepository.MovieWithNameExists(request.Title);

        if (movieExists)
        {
            return Result.Failure<Guid>(MovieErrors.MovieWithNameExists);
        }
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

        // Generate unique file name
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Poster.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.Poster.CopyToAsync(stream);
        }

        var relativePath = $"/uploads/{fileName}";
        var movie = new Movie(request.Title, relativePath);
        try
        {
            await _movieRepository.AddAsync(movie);
            return Result.Success(movie.Id);
        }
        catch (Exception e)
        {
            return Result.Failure<Guid>(Error.FromException(e));
        }
    }
}
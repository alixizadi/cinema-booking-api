using CinemaApp.Domain.Abstractions;

namespace CinemaApp.Domain.Entities;

public class Movie : BaseEntity
{
    public string Title { get; private set; }
    public string PosterUrl { get; private set; }

    private Movie()
    {
    }

    public Movie(string title, string posterUrl)
    {
        Title = title;
        PosterUrl = posterUrl;
    }

    public void UpdateDetails(string title, string posterUrl)
    {
        Title = title;
        PosterUrl = posterUrl;
    }
}

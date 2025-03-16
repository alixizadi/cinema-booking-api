using CinemaApp.Application.Abstractions.Messaging;

namespace CinemaApp.Application.Commands.BookSeatCommand;

public sealed class BookSeatCommand  : ICommand<List<Guid>>{
    public Guid UserId { get; set; }
    public Guid MovieScheduleId { get; set; }
    public List<BookingSeat> Seats { get; set; } = [];
}

public record BookingSeat
{
    public int RowNumber { get; set; }
    public int ColNumber { get; set; }
}
using CinemaApp.Domain.Abstractions;

namespace CinemaApp.Domain.Errors;

public class UserErrors
{
    public static Error UserNotFound = new ("Error.UserNotFound", "User not found");
    public static Error EmailAlreadyExists = new ("Error.EmailAlreadyExists", "Email is already registered");
    public static Error InvalidCredentials = new Error("Error.InvalidCredentials", "Invalid credentials.");
}

public class RoomErrors
{
    public static Error RoomNotFound = new("Error.RoomNotFound", "Room not found");
    public static Error RoomWithNameExists = new("Error.RoomWithNameExists", "Room with name already exists");
}

public class MovieErrors
{
    public static Error MovieNotFound = new("Error.MovieNotFound", "Movie not found");
    public static Error MovieWithNameExists = new("Error.MovieWithNameExists", "Movie with name already exists");
}

public class ScheduleErrors
{
    public static Error NoSchedulesFound = new ("Error.NoSchedulesFound", "No schedules found for this room.");
    public static Error ScheduleNotFound = new("Error.NoSchedulesFound", "Movie Schedule not found.");
    public static Error TimesNotCorrect = new("Error.TimesNotCorrect", "End time should be after start time");
    public static Error ScheduleOverlap = new("Error.ScheduleOverlap", "Time period is overlapping with another schedule");
}

public class BookingErrors
{
    public static Error SeatNotFound = new ("Error.SeatNotFound", "Seat not found");
    public static Error SeatAlreadyBooked = new("Error.SeatAlreadyBooked", "Seat is already booked");

}
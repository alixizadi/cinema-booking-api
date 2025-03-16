namespace CinemaApp.Domain.Abstractions;

public record Error (string Code, string Message)
{
    public static Error None = new Error(string.Empty, string.Empty);
    public static Error BaseError = new ("Error.Base", "Default error");
    public static Error NullValue = new ("Error.NullValue", "Value cannot be null");

    
    public static Error NotFound(string message) => new("NotFound", message);
    public static Error BadRequest(string message) => new("BadRequest", message);
    public static Error InternalServerError(string message) => new("InternalServerError", message);
    public static Error Unauthorized(string message) => new("Unauthorized", message);
    
    public static Error FromException(Exception exception) => new("Error.Exception", exception.Message);
    
}
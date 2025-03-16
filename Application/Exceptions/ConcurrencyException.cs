namespace Account.Application.Exceptions;

public class ConcurrencyException : Exception
{
    public ConcurrencyException(string message, Exception exception)
    : base(message, exception)
    {
        
    }
}
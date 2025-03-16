using System.Diagnostics.CodeAnalysis;

namespace CinemaApp.Domain.Abstractions;

public class Result
{
 
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("Successful result cannot have an error.");
        }
        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("Failed result must have an error.");
        }
        
        IsSuccess = isSuccess;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }
    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);
    
    public static Result<T> Success<T>(T value) => new Result<T>(value, true, Error.None);
    public static Result<T> Failure<T>(Error error) => new Result<T>(default!, false, error);

    public static Result<T> Create<T>(T? value)
    {
        return value is not null ? Success<T>(value) : Failure<T>(Error.NullValue);
    }
}

public class Result<T> : Result
{
    private readonly T? _value;
    protected internal Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }
    
    [NotNull]
    public T Value => IsSuccess 
        ? _value! 
        : throw new InvalidOperationException("No value for failure result."); 

    
    public static implicit operator Result<T>(T? value) => Create(value);
}
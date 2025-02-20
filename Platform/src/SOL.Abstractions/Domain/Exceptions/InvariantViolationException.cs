namespace SOL.Abstractions.Domain;

public class InvariantViolationException : InvalidOperationException
{
    public InvariantViolationException() 
        : base()
    { }

    public InvariantViolationException(string? message)
        : base(message)
    { }

    public InvariantViolationException(string? message, Exception? innerException)
        : base(message, innerException)
    { }
}
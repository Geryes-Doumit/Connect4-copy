namespace Connect4.Domain.Exceptions;

/// <summary>
/// Represents a domain exception indicating that the user is trying to perform an operation that is not allowed because it is not their turn.
/// </summary>
public class NotYourTurnDomainException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotYourTurnDomainException"/> class.
    /// </summary>
    /// <param name="message">A description of the exception.</param>
    public NotYourTurnDomainException(string message) : base(message)
    {
    }
}

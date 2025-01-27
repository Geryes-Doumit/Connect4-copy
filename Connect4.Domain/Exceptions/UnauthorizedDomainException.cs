namespace Connect4.Domain.Exceptions;

/// <summary>
/// Represents a domain exception indicating that the current request is unauthorized (401 status).
/// </summary>
public class UnauthorizedDomainException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedDomainException"/> class.
    /// </summary>
    /// <param name="message">A description of the exception.</param>
    public UnauthorizedDomainException(string message) : base(message)
    {
    }
}

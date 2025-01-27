namespace Connect4.Domain.Exceptions;

/// <summary>
/// Represents a domain exception indicating that the requested game was not found.
/// </summary>
public class GameNotFoundException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameNotFoundException"/> class.
    /// </summary>
    /// <param name="message">A description of the exception.</param>
    public GameNotFoundException(string message) : base(message)
    {
    }
}

namespace Connect4.Domain.Exceptions;

/// <summary>
/// Represents a domain exception indicating that the user is busy and cannot perform the requested operation.
/// </summary>
public class BusyUserDomainException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BusyUserDomainException"/> class.
    /// </summary>
    /// <param name="message">A description of the exception.</param>
    public BusyUserDomainException(string message, int? gameId) : base(message)
    {
        GameId = gameId ?? 0;
    }

    public int GameId { get; set; }
}
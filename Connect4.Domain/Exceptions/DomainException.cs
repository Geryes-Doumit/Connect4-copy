using System.Net;

namespace Connect4.Domain.Exceptions;

/// <summary>
/// Represents a generic domain exception where the default behavior is to treat it as a client-side error (400 Bad Request).
/// </summary>
/// <remarks>
/// <para>
/// This exception is used to indicate errors that occur due to invalid operations or inputs within the domain logic.
/// It is typically treated as a client-side error, resulting in a 400 Bad Request HTTP response.
/// </para>
/// </remarks>
public class DomainException: Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DomainException(string message) : base(message) 
    {
        StatusCode = HttpStatusCode.BadRequest;
    }

    /// <summary>
    /// Gets the default HTTP status code associated with this domain exception.
    /// </summary>
    /// <remarks>
    /// This property is used to determine the HTTP response status code when this exception is thrown.
    /// For <see cref="DomainException"/>, the default status code is <see cref="HttpStatusCode.BadRequest"/> (400).
    /// </remarks>
    public HttpStatusCode StatusCode { get; }
}
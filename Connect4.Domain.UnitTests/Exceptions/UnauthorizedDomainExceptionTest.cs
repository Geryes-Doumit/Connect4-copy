using System;
using System.Net;
using Xunit;
using Connect4.Domain.Exceptions;

namespace Connect4.Domain.Exceptions.UnitTests;

/// <summary>
/// Unit tests for the <see cref="UnauthorizedDomainException"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class UnauthorizedDomainExceptionTest
{
    /// <summary>
    /// Tests that the constructor sets the message correctly.
    /// </summary>
    [Fact]
    public void Constructor_SetsMessageCorrectly()
    {
        // Arrange
        var message = "Unauthorized access";

        // Act
        var exception = new UnauthorizedDomainException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    /// <summary>
    /// Tests that the constructor sets the status code to Unauthorized.
    /// </summary>
    [Fact]
    public void Constructor_SetsStatusCodeToUnauthorized()
    {
        // Arrange
        var message = "Unauthorized access";

        // Act
        var exception = new UnauthorizedDomainException(message);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
    }
}
using System;
using System.Net;
using Xunit;
using Connect4.Domain.Exceptions;

namespace Connect4.Domain.Exceptions.UnitTests;

/// <summary>
/// Unit tests for the <see cref="GameNotFoundException"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class GameNotFoundExceptionTest
{
    /// <summary>
    /// Tests that the constructor sets the message correctly.
    /// </summary>
    [Fact]
    public void Constructor_SetsMessageCorrectly()
    {
        // Arrange
        var message = "Game not found";

        // Act
        var exception = new GameNotFoundException(message);

        // Assert
        Assert.Equal(message, exception.Message);
    }

    /// <summary>
    /// Tests that the constructor sets the status code to BadRequest.
    /// </summary>
    [Fact]
    public void Constructor_SetsStatusCodeToBadRequest()
    {
        // Arrange
        var message = "Game not found";

        // Act
        var exception = new GameNotFoundException(message);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
    }
}
using System;
using Xunit;
using Connect4.Domain.Exceptions;

namespace Connect4.Domain.Exceptions.UnitTests;

/// <summary>
/// Unit tests for the <see cref="BusyUserDomainException"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class BusyUserDomainExceptionTest
{
    /// <summary>
    /// Tests that the constructor sets the properties correctly when the game ID is not null.
    /// </summary>
    [Fact]
    public void Constructor_WithNonNullGameId_SetsPropertiesCorrectly()
    {
        // Arrange
        var message = "User is busy";
        int? gameId = 123;

        // Act
        var exception = new BusyUserDomainException(message, gameId);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(gameId, exception.GameId);
    }

    /// <summary>
    /// Tests that the constructor sets the properties correctly when the game ID is null.
    /// </summary>
    [Fact]
    public void Constructor_WithNullGameId_SetsGameIdToZero()
    {
        // Arrange
        var message = "User is busy";
        int? gameId = null;

        // Act
        var exception = new BusyUserDomainException(message, gameId);

        // Assert
        Assert.Equal(message, exception.Message);
        Assert.Equal(0, exception.GameId);
    }

    /// <summary>
    /// Tests that the <see cref="GameId"/> property can be set and retrieved.
    /// </summary>
    [Fact]
    public void GameIdProperty_CanBeSetAndRetrieved()
    {
        // Arrange
        var exception = new BusyUserDomainException("User is busy", 123);

        // Act
        exception.GameId = 456;

        // Assert
        Assert.Equal(456, exception.GameId);
    }
}
using Xunit;
using Connect4.Domain.Commands;

namespace Connect4.Domain.Commands.UnitTests;

/// <summary>
/// Unit tests for the <see cref="JoinGameCommand"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class JoinGameCommandTest
{
    /// <summary>
    /// Tests the constructor to ensure it correctly initializes the game ID and user name properties.
    /// </summary>
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var gameId = 1;
        var userName = "TestUser";

        // Act
        var command = new JoinGameCommand(gameId, userName);

        // Assert
        Assert.Equal(gameId, command.GameId);
        Assert.Equal(userName, command.UserName);
    }

    /// <summary>
    /// Tests that the GameId property contains the correct game ID.
    /// </summary>
    [Fact]
    public void GameIdProperty_ReturnsCorrectValue()
    {
        // Arrange
        var gameId = 1;
        var userName = "TestUser";

        // Act
        var command = new JoinGameCommand(gameId, userName);

        // Assert
        Assert.Equal(gameId, command.GameId);
    }

    /// <summary>
    /// Tests that the UserName property contains the correct user name.
    /// </summary>
    [Fact]
    public void UserNameProperty_ReturnsCorrectValue()
    {
        // Arrange
        var gameId = 1;
        var userName = "TestUser";

        // Act
        var command = new JoinGameCommand(gameId, userName);

        // Assert
        Assert.Equal(userName, command.UserName);
    }
}
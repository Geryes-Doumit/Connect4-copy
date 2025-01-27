using Xunit;
using Connect4.Domain.Commands;

namespace Connect4.Domain.Commands.UnitTests;

/// <summary>
/// Unit tests for the <see cref="PlayMoveCommand"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class PlayMoveCommandTest
{
    /// <summary>
    /// Tests that the constructor sets the properties correctly.
    /// </summary>
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var gameId = 1;
        var username = "TestUser";
        var column = 3;

        // Act
        var command = new PlayMoveCommand(gameId, username, column);

        // Assert
        Assert.Equal(gameId, command.GameId);
        Assert.Equal(username, command.Username);
        Assert.Equal(column, command.Column);
    }

    /// <summary>
    /// Tests that the <see cref="PlayMoveCommand.GameId"/> property returns the correct value.
    /// </summary>
    [Fact]
    public void GameIdProperty_ReturnsCorrectValue()
    {
        // Arrange
        var gameId = 1;
        var username = "TestUser";
        var column = 3;

        // Act
        var command = new PlayMoveCommand(gameId, username, column);

        // Assert
        Assert.Equal(gameId, command.GameId);
    }

    /// <summary>
    /// Tests that the <see cref="PlayMoveCommand.Username"/> property returns the correct value.
    /// </summary>
    [Fact]
    public void UsernameProperty_ReturnsCorrectValue()
    {
        // Arrange
        var gameId = 1;
        var username = "TestUser";
        var column = 3;

        // Act
        var command = new PlayMoveCommand(gameId, username, column);

        // Assert
        Assert.Equal(username, command.Username);
    }

    /// <summary>
    /// Tests that the <see cref="PlayMoveCommand.Column"/> property returns the correct value.
    /// </summary>
    [Fact]
    public void ColumnProperty_ReturnsCorrectValue()
    {
        // Arrange
        var gameId = 1;
        var username = "TestUser";
        var column = 3;

        // Act
        var command = new PlayMoveCommand(gameId, username, column);

        // Assert
        Assert.Equal(column, command.Column);
    }
}
using Xunit;
using Connect4.Common.Model;
using Connect4.Domain.Commands;

namespace Connect4.Domain.Commands.UnitTests;

/// <summary>
/// Unit tests for the <see cref="CreateGameCommand"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class CreateGameCommandTest
{
    /// <summary>
    /// Tests the constructor to ensure it correctly initializes the game property.
    /// </summary>
    [Fact]
    public void Constructor_SetsGamePropertyCorrectly()
    {
        // Arrange
        var gameName = "TestGame";
        var hostName = "TestHost";

        // Act
        var command = new CreateGameCommand(gameName, hostName);

        // Assert
        Assert.NotNull(command.Game);
        Assert.Equal(gameName, command.Game.Name);
        Assert.Equal(hostName, command.Game.Host);
        Assert.Equal("WaitingForPlayers", command.Game.Status);
        Assert.Equal(hostName, command.Game.Board.CurrentPlayer);
        Assert.Equal("0000000;0000000;0000000;0000000;0000000;0000000", command.Game.Board.State);
    }

    /// <summary>
    /// Tests that the Game property contains the correct game name.
    /// </summary>
    [Fact]
    public void GameProperty_ContainsCorrectGameName()
    {
        // Arrange
        var gameName = "TestGame";
        var hostName = "TestHost";

        // Act
        var command = new CreateGameCommand(gameName, hostName);

        // Assert
        Assert.Equal(gameName, command.Game.Name);
    }

    /// <summary>
    /// Tests that the Game property contains the correct host name.
    /// </summary>
    [Fact]
    public void GameProperty_ContainsCorrectHostName()
    {
        // Arrange
        var gameName = "TestGame";
        var hostName = "TestHost";

        // Act
        var command = new CreateGameCommand(gameName, hostName);

        // Assert
        Assert.Equal(hostName, command.Game.Host);
    }

    /// <summary>
    /// Tests that the initial game status is set to "WaitingForPlayers".
    /// </summary>
    [Fact]
    public void GameProperty_HasCorrectInitialState()
    {
        // Arrange
        var gameName = "TestGame";
        var hostName = "TestHost";

        // Act
        var command = new CreateGameCommand(gameName, hostName);

        // Assert
        Assert.Equal("WaitingForPlayers", command.Game.Status);
    }

    /// <summary>
    /// Tests that the initial board state is set correctly.
    /// </summary>
    [Fact]
    public void GameProperty_HasCorrectInitialBoardState()
    {
        // Arrange
        var gameName = "TestGame";
        var hostName = "TestHost";

        // Act
        var command = new CreateGameCommand(gameName, hostName);

        // Assert
        Assert.Equal("0000000;0000000;0000000;0000000;0000000;0000000", command.Game.Board.State);
    }
}
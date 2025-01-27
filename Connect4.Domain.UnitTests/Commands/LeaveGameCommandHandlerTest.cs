using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Connect4.Domain.Commands;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using Connect4.Common.Model;

namespace Connect4.Domain.Commands.UnitTests;

/// <summary>
/// Unit tests for the <see cref="LeaveGameCommandHandler"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class LeaveGameCommandHandlerTest
{
    private readonly Mock<GameRepository> _gameRepositoryMock;
    private readonly Mock<GameQueryRepository> _gameQueryRepositoryMock;
    private readonly LeaveGameCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeaveGameCommandHandlerTest"/> class.
    /// </summary>
    public LeaveGameCommandHandlerTest()
    {
        _gameRepositoryMock = new Mock<GameRepository>();
        _gameQueryRepositoryMock = new Mock<GameQueryRepository>();
        _handler = new LeaveGameCommandHandler(_gameRepositoryMock.Object, _gameQueryRepositoryMock.Object);
    }

    /// <summary>
    /// Tests the Handle method to ensure it throws a <see cref="GameNotFoundException"/>
    /// </summary>
    [Fact]
    public async Task Handle_GameNotFound_ThrowsGameNotFoundException()
    {
        // Arrange
        var command = new LeaveGameCommand(1, "TestUser");
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync((GameDetail)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GameNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Game with id 1 not found.", exception.Message);
    }

    /// <summary>
    /// Tests the Handle method to ensure it deletes a game when the game is waiting for players and the host leaves.
    /// </summary>
    [Fact]
    public async Task Handle_GameWaitingForPlayersAndHostLeaves_DeletesGame()
    {
        // Arrange
        var command = new LeaveGameCommand(1, "TestHost");
        var gameDetail = new GameDetail (new Connect4Game( "Test Game", "TestHost", "WaitingForPlayers", new ("TestHost", "000...")));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.Delete(1), Times.Once);
    }

    /// <summary>
    /// Tests the Handle method to ensure it finishes a game when the game is in progress and the host leaves.
    /// </summary>
    [Fact]
    public async Task Handle_GameInProgressAndHostLeaves_FinishesGameWithGuestAsWinner()
    {
        // Arrange
        var command = new LeaveGameCommand(1, "TestHost");
        var gameDetail = new GameDetail (new Connect4Game( "Test Game", "TestHost", "InProgress", new ("TestHost", "000..."), "TestGuest"));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.Finished(1, "TestGuest"), Times.Once);
    }

    /// <summary>
    /// Tests the Handle method to ensure it finishes a game when the game is in progress and the guest leaves.
    /// </summary>
    [Fact]
    public async Task Handle_GameInProgressAndGuestLeaves_FinishesGameWithHostAsWinner()
    {
        // Arrange
        var command = new LeaveGameCommand(1, "TestGuest");
        var gameDetail = new GameDetail (new Connect4Game( "Test Game", "TestHost", "InProgress", new ("TestHost", "000..."), "TestGuest"));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.Finished(1, "TestHost"), Times.Once);
    }
}
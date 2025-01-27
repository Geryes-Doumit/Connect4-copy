using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Connect4.Domain.Commands;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using Connect4.Domain.Services;
using Connect4.Common.Model;

namespace Connect4.Domain.Commands.UnitTests;

/// <summary>
/// Unit tests for the <see cref="PlayMoveCommandHandler"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class PlayMoveCommandHandlerTest
{
    private readonly Mock<GameQueryRepository> _gameQueryRepositoryMock;
    private readonly Mock<GameRepository> _gameRepositoryMock;
    private readonly Mock<BoardQueryRepository> _boardQueryRepositoryMock;
    private readonly Mock<BoardRepository> _boardRepositoryMock;
    private readonly Mock<IGameService> _gameServiceMock;
    private readonly PlayMoveCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayMoveCommandHandlerTest"/> class.
    /// </summary>
    public PlayMoveCommandHandlerTest()
    {
        _gameQueryRepositoryMock = new Mock<GameQueryRepository>();
        _gameRepositoryMock = new Mock<GameRepository>();
        _boardQueryRepositoryMock = new Mock<BoardQueryRepository>();
        _boardRepositoryMock = new Mock<BoardRepository>();
        _gameServiceMock = new Mock<IGameService>();
        _handler = new PlayMoveCommandHandler(
            _gameQueryRepositoryMock.Object,
            _gameRepositoryMock.Object,
            _boardQueryRepositoryMock.Object,
            _boardRepositoryMock.Object,
            _gameServiceMock.Object
        );
    }

    /// <summary>
    /// Tests that the <see cref="PlayMoveCommandHandler.Handle"/> method throws a <see cref="GameNotFoundException"/> when the game is not found.
    /// </summary>
    [Fact]
    public async Task Handle_GameNotFound_ThrowsGameNotFoundException()
    {
        // Arrange
        var command = new PlayMoveCommand(1, "TestUser", 3);
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync((GameDetail)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GameNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Game with id 1 not found.", exception.Message);
    }

    /// <summary>
    /// Tests that the <see cref="PlayMoveCommandHandler.Handle"/> method throws a <see cref="NotYourTurnDomainException"/> when it is not the user's turn.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotInGame_ThrowsGameNotFoundException()
    {
        // Arrange
        var command = new PlayMoveCommand(1, "TestUser", 3);
        var gameDetail = new GameDetail (new Connect4Game(1, "Test Game", "HostUser", "InProgress", new ("TestHost", "000..."), "GuestUser"));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GameNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Game with id 1 not found.", exception.Message);
    }

    /// <summary>
    /// Tests that the <see cref="PlayMoveCommandHandler.Handle"/> method throws a <see cref="NotYourTurnDomainException"/> when it is not the user's turn.
    /// </summary>
    [Fact]
    public async Task Handle_NotUserTurn_ThrowsNotYourTurnDomainException()
    {
        // Arrange
        var command = new PlayMoveCommand(1, "HostUser", 3);
        var gameDetail = new GameDetail (new Connect4Game(1, "Test Game", "HostUser", "InProgress", new ("GuestUser", "000..."), "GuestUser"));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotYourTurnDomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("It is not your turn to play.", exception.Message);
    }

    /// <summary>
    /// Tests that the <see cref="PlayMoveCommandHandler.Handle"/> method throws a <see cref="DomainException"/> when the move is invalid.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidMove_ThrowsDomainException()
    {
        // Arrange
        var command = new PlayMoveCommand(1, "HostUser", 3);
        var gameDetail = new GameDetail (new Connect4Game(1, "Test Game", "HostUser", "InProgress", new ("HostUser", "000..."), "GuestUser"));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);
        _gameServiceMock.Setup(service => service.ValidateMove(It.IsAny<Board>(), 3))
                        .Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Invalid move.", exception.Message);
    }

    /// <summary>
    /// Tests that the <see cref="PlayMoveCommandHandler.Handle"/> method updates the board and returns the game ID.
    /// </summary>
    [Fact]
    public async Task Handle_ValidMove_UpdatesBoardAndReturnsGameId()
    {
        // Arrange
        var command = new PlayMoveCommand(1, "HostUser", 3);
        var gameDetail = new GameDetail (new Connect4Game(1, "Test Game", "HostUser", "InProgress", new ("HostUser", "000..."), "GuestUser"));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);
        _gameServiceMock.Setup(service => service.ValidateMove(It.IsAny<Board>(), 3))
                        .Returns(true);
        _gameServiceMock.Setup(service => service.UpdateBoard(It.IsAny<Board>(), '1', 3))
                        .Returns("UpdatedBoardState");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
        _boardRepositoryMock.Verify(repo => repo.UpdateBoard(1, "UpdatedBoardState", "GuestUser"), Times.Once);
    }

    /// <summary>
    /// Tests that the <see cref="PlayMoveCommandHandler.Handle"/> method updates the board, finishes the game, and returns the game ID.
    /// </summary>
    [Fact]
    public async Task Handle_ValidMoveAndWin_UpdatesBoardAndFinishesGame()
    {
        // Arrange
        var command = new PlayMoveCommand(1, "HostUser", 3);
        var gameDetail = new GameDetail (new Connect4Game(1, "Test Game", "HostUser", "InProgress", new ("HostUser", "000..."), "GuestUser"));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);
        _gameServiceMock.Setup(service => service.ValidateMove(It.IsAny<Board>(), 3))
                        .Returns(true);
        _gameServiceMock.Setup(service => service.UpdateBoard(It.IsAny<Board>(), '1', 3))
                        .Returns("UpdatedBoardState");
        _gameServiceMock.Setup(service => service.CheckWin(It.IsAny<Board>(), '1'))
                        .Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
        _boardRepositoryMock.Verify(repo => repo.UpdateBoard(1, "UpdatedBoardState", "GuestUser"), Times.Once);
        _gameRepositoryMock.Verify(repo => repo.Finished(1, "HostUser"), Times.Once);
    }
}
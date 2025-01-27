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
/// Unit tests for the <see cref="JoinGameCommandHandler"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class JoinGameCommandHandlerTest
{
    private readonly Mock<GameRepository> _gameRepositoryMock;
    private readonly Mock<GameQueryRepository> _gameQueryRepositoryMock;
    private readonly Mock<IPlayerStatusService> _playerStatusMock;
    private readonly JoinGameCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="JoinGameCommandHandlerTest"/> class.
    /// </summary>
    public JoinGameCommandHandlerTest()
    {
        _gameRepositoryMock = new Mock<GameRepository>();
        _gameQueryRepositoryMock = new Mock<GameQueryRepository>();
        _playerStatusMock = new Mock<IPlayerStatusService>();
        _handler = new JoinGameCommandHandler(_gameRepositoryMock.Object, _playerStatusMock.Object, _gameQueryRepositoryMock.Object);
    }

    /// <summary>
    /// Tests the Handle method to ensure it throws a <see cref="BusyUserDomainException"/>
    /// </summary>
    [Fact]
    public async Task Handle_UserIsBusy_ThrowsBusyUserDomainException()
    {
        // Arrange
        var command = new JoinGameCommand(1, "TestUser");
        _playerStatusMock.Setup(service => service.CheckIfUserIsBusy("TestUser"))
                         .ReturnsAsync(1);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusyUserDomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("You are busy in game 1, cannot join a new game.", exception.Message);
        Assert.Equal(1, exception.GameId);
    }

    /// <summary
    /// Tests the Handle method to ensure it throws a <see cref="GameNotFoundException"/>
    /// </summary>
    [Fact]
    public async Task Handle_GameNotFound_ThrowsGameNotFoundException()
    {
        // Arrange
        var command = new JoinGameCommand(1, "TestUser");
        _playerStatusMock.Setup(service => service.CheckIfUserIsBusy("TestUser"))
                         .ReturnsAsync((int?)null);
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync((GameDetail)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GameNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Game with id 1 not found.", exception.Message);
    }


    /// <summary>
    /// Tests the Handle method to ensure it throws a <see cref="GameNotFoundException"/>
    /// </summary>
    [Fact]
    public async Task Handle_GameNotInJoinableState_ThrowsGameNotFoundException()
    {
        // Arrange
        var command = new JoinGameCommand(1, "TestUser");
        _playerStatusMock.Setup(service => service.CheckIfUserIsBusy("TestUser"))
                         .ReturnsAsync((int?)null);
        var gameDetail = new GameDetail (new Connect4Game( "Test Game", "TestUser", "InProgress", new ("TestUser", "000...")));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<GameNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Game with id 1 not found.", exception.Message);
    }

    /// <summary>
    /// Tests the Handle method to ensure it validates the user and joins the game successfully.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_JoinsGameSuccessfully()
    {
        // Arrange
        var command = new JoinGameCommand(1, "TestUser");
        _playerStatusMock.Setup(service => service.CheckIfUserIsBusy("TestUser"))
                         .ReturnsAsync((int?)null);
        var gameDetail = new GameDetail (new Connect4Game( "Test Game", "TestUser", "WaitingForPlayers", new ("TestUser", "000...")));
        _gameQueryRepositoryMock.Setup(repo => repo.GetOne(1))
                                .ReturnsAsync(gameDetail);
        _gameRepositoryMock.Setup(repo => repo.InProgress(1, "TestUser"))
                           .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
        _gameRepositoryMock.Verify(repo => repo.InProgress(1, "TestUser"), Times.Once);
    }
}
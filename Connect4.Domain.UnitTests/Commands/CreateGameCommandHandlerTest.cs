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
/// Unit tests for the <see cref="CreateGameCommandHandler"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class CreateGameCommandHandlerTest
{
    private readonly Mock<IPlayerStatusService> _playerStatusMock;
    private readonly Mock<GameRepository> _gameRepositoryMock;
    private readonly CreateGameCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGameCommandHandlerTest"/> class.
    /// </summary>
    public CreateGameCommandHandlerTest()
    {
        _playerStatusMock = new Mock<IPlayerStatusService>();
        _gameRepositoryMock = new Mock<GameRepository>();
        _handler = new CreateGameCommandHandler(_gameRepositoryMock.Object, _playerStatusMock.Object);
    }

    /// <summary>
    /// Tests the Handle method to ensure it throws a <see cref="BusyUserDomainException"/>
    /// when the user is already busy in another game.
    /// </summary>
    [Fact]
    public async Task Handle_UserIsBusy_ThrowsBusyUserDomainException()
    {
        // Arrange
        var command = new CreateGameCommand("TestGame", "TestHost");
        _playerStatusMock.Setup(service => service.CheckIfUserIsBusy("TestHost"))
                         .ReturnsAsync(1);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusyUserDomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("You are busy in game 1, cannot create a new game.", exception.Message);
        Assert.Equal(1, exception.GameId);
    }

    /// <summary>
    /// Tests the Handle method to ensure it successfully creates a new game when the user is not busy.
    /// </summary>
    [Fact]
    public async Task Handle_UserIsNotBusy_CreatesGame()
    {
        // Arrange
        var command = new CreateGameCommand("TestGame", "TestHost");
        _playerStatusMock.Setup(service => service.CheckIfUserIsBusy("TestHost"))
                         .ReturnsAsync((int?)null);
        _gameRepositoryMock.Setup(repo => repo.Create(It.IsAny<Connect4Game>()))
                           .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(1, result);
        _gameRepositoryMock.Verify(repo => repo.Create(It.IsAny<Connect4Game>()), Times.Once);
    }
}

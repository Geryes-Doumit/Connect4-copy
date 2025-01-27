using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using Connect4.Domain.Services;
using Connect4.Common.Model;
using System.Collections.Generic;

namespace Connect4.Domain.Queries.UnitTests;

/// <summary>
/// Unit tests for <see cref="GetWaitingGamesQueryHandler"/>."/>
/// </summary>
[Trait("Category", "Unit")]
public class GetWaitingGamesQueryHandlerTest
{
    private readonly Mock<GameQueryRepository> _gameRepositoryMock;
    private readonly Mock<IPlayerStatusService> _playerStatusMock;
    private readonly GetWaitingGamesQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetWaitingGamesQueryHandlerTest"/> class.
    /// </summary>
    public GetWaitingGamesQueryHandlerTest()
    {
        _gameRepositoryMock = new Mock<GameQueryRepository>() { CallBase = false };
        _playerStatusMock = new Mock<IPlayerStatusService>();
        _handler = new GetWaitingGamesQueryHandler(
            _gameRepositoryMock.Object,
            _playerStatusMock.Object
        );
    }

    /// <summary>
    /// Tests is the method throws a <see cref="BusyUserDomainException"/> when the user is busy and tries to access waiting games.
    /// </summary>
    [Fact]
    public async Task Handle_UserIsBusy_ThrowsBusyUserDomainException()
    {
        // Arrange
        var query = new GetWaitingGamesQuery("waiting") { UserName = "User" };
        _playerStatusMock.Setup(p => p.CheckIfUserIsBusy("User"))
                         .ReturnsAsync(1);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BusyUserDomainException>(() =>
            _handler.Handle(query, CancellationToken.None));
        Assert.Contains("You are busy in game 1", ex.Message);
    }

    /// <summary>
    /// Tests if the method returns the waiting games when the user is not busy.
    /// </summary>
    [Fact]
    public async Task Handle_UserIsNotBusy_ReturnsWaitingGames()
    {
        // Arrange
        var query = new GetWaitingGamesQuery("waiting", 2, 1) { UserName = "Marc" };
        _playerStatusMock.Setup(p => p.CheckIfUserIsBusy("Marc"))
                         .ReturnsAsync(() => null);

        var expectedGames = new List<WaitingGameDto>
        {
            new WaitingGameDto ( new Connect4Game(1, "Game1", "Marc", "WaitingForPlayers", new ("Marc", "0000000..."))),
            new WaitingGameDto ( new Connect4Game(2, "Game2", "Marc", "WaitingForPlayers", new ("Marc", "0000000...")))
        };

        _gameRepositoryMock.Setup(repo => repo.Find<WaitingGameDto>(
            query.LimitOrDefault,
            query.OffsetOrDefault,
            It.IsAny<GameListFilterSpecification>()))
            .ReturnsAsync(expectedGames);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(2, result?.Count());
        Assert.Contains(result, g => g.GameName == "Game1");
        Assert.Contains(result, g => g.GameName == "Game2");
    }

    /// <summary>
    /// Tests if the method verifies the limit and offset when querying the waiting games.
    /// </summary>
    [Fact]
    public async Task Handle_VerifyLimitAndOffset()
    {
        // Arrange
        var query = new GetWaitingGamesQuery("waiting", null, null) { UserName = "User" };
        _playerStatusMock.Setup(p => p.CheckIfUserIsBusy("User")).ReturnsAsync(() => null);

        List<WaitingGameDto> emptyList = new();
        _gameRepositoryMock.Setup(repo => repo.Find<WaitingGameDto>(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<GameListFilterSpecification>()))
            .ReturnsAsync(emptyList);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.Find<WaitingGameDto>(
            10, // Default limit
            0,  // Default offset
            It.IsAny<GameListFilterSpecification>()),
            Times.Once);
    }
}
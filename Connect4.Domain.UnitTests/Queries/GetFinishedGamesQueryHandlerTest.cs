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
/// Unit tests for <see cref="GetFinishedGamesQueryHandler"/>
/// </summary>
[Trait("Category", "Unit")]
public class GetFinishedGamesQueryHandlerTest
{
    private readonly Mock<GameQueryRepository> _gameRepositoryMock;
    private readonly Mock<IPlayerStatusService> _playerStatusMock;
    private readonly GetFinishedGamesQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFinishedGamesQueryHandlerTest"/> class.
    /// </summary>
    public GetFinishedGamesQueryHandlerTest()
    {
        _gameRepositoryMock = new Mock<GameQueryRepository>() { CallBase = false };
        _playerStatusMock = new Mock<IPlayerStatusService>();
        _handler = new GetFinishedGamesQueryHandler(
            _gameRepositoryMock.Object,
            _playerStatusMock.Object
        );
    }

    /// <summary>
    /// Test if the method throws a <see cref="BusyUserDomainException"/> when the user is busy and tries to access finished games.
    /// </summary>
    [Fact]
    public async Task Handle_UserIsBusy_ThrowsBusyUserDomainException()
    {
        // Arrange
        var query = new GetFinishedGamesQuery("finished-User") { UserName = "User" };
        _playerStatusMock.Setup(p => p.CheckIfUserIsBusy("User")).ReturnsAsync(1);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<BusyUserDomainException>(() =>
            _handler.Handle(query, CancellationToken.None));
        Assert.Contains("You are busy in game 1", ex.Message);
    }

    /// <summary>
    /// Test if the method returns finished games when the user is not busy.
    /// </summary>
    [Fact]
    public async Task Handle_UserIsNotBusy_ReturnsFinishedGames()
    {
        // Arrange
        var query = new GetFinishedGamesQuery("finished-Marc", 2, 1) { UserName = "Marc" };
        _playerStatusMock.Setup(p => p.CheckIfUserIsBusy("Marc"))
                         .ReturnsAsync(() => null);

        var expectedGames = new List<FinishedGameDto>
        {
            new FinishedGameDto (new Connect4Game(1, "Game1", "Marc", "Finished", new ("Marc", "0000000..."))),
            new FinishedGameDto (new Connect4Game(2, "Game2", "Marc", "Finished", new ("Marc", "0000000...")))
        };

        _gameRepositoryMock.Setup(repo => repo.Find<FinishedGameDto>(
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
    /// Test if the method verifies the limit and offset values when querying the repository.
    /// </summary>
    [Fact]
    public async Task Handle_VerifyLimitAndOffset()
    {
        // Arrange
        var query = new GetFinishedGamesQuery("finished-User", null, null) { UserName = "User" };
        _playerStatusMock.Setup(p => p.CheckIfUserIsBusy("User")).ReturnsAsync(() => null);

        List<FinishedGameDto> emptyList = new();
        _gameRepositoryMock.Setup(repo => repo.Find<FinishedGameDto>(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<GameListFilterSpecification>()))
            .ReturnsAsync(emptyList);

        // Act
        await _handler.Handle(query, CancellationToken.None);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.Find<FinishedGameDto>(
            10, // Default limit
            0,  // Default offset
            It.IsAny<GameListFilterSpecification>()),
            Times.Once);
    }
}
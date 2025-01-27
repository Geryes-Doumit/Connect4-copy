using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Connect4.Domain.Repositories;
using Connect4.Common.Model;

namespace Connect4.Domain.Services.UnitTests;

/// <summary>
/// Unit tests for the <see cref="PlayerStatusService"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class PlayerStatusServiceTest
{
    private readonly Mock<GameQueryRepository> _gameRepoMock;
    private readonly IPlayerStatusService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerStatusServiceTest"/> class.
    /// </summary>
    public PlayerStatusServiceTest()
    {
        _gameRepoMock = new Mock<GameQueryRepository>();
        _service = new PlayerStatusService(_gameRepoMock.Object);
    }

    /// <summary>
    /// Tests if the method detect that a user is busy in a waiting game.
    /// </summary>
    [Fact]
    public async Task CheckIfUserIsBusy_UserInWaitingGame_ReturnsGameId()
    {
        // Arrange
        var userName = "testUser";
        var waitingGame = new WaitingGameDto(new Connect4Game(1, "Game test 1", userName, "WaitingForPlayers", new Board(userName, "0000000;0000000;0000000;0000000;0000000;0000000")) );
        _gameRepoMock.Setup(repo => repo.FindAll<WaitingGameDto>(It.IsAny<BusyGameSpecification>()))
                     .ReturnsAsync(new List<WaitingGameDto> { waitingGame });
        _gameRepoMock.Setup(repo => repo.FindAll<GameDetail>(It.IsAny<BusyGameSpecification>()))
                     .ReturnsAsync(new List<GameDetail>());

        // Act
        var result = await _service.CheckIfUserIsBusy(userName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result);
    }

    /// <summary>
    /// Tests if the method detect that a user is busy in an in-progress game.
    /// </summary>
    [Fact]
    public async Task CheckIfUserIsBusy_UserInInProgressGame_ReturnsGameId()
    {
        // Arrange
        var userName = "testUser";
        var inProgressGame = new GameDetail(new Connect4Game(2, "Game test 1", userName, "InProgress", new Board(userName, "0000000;0000000;0000000;0000000;0000000;0000000")) );
        _gameRepoMock.Setup(repo => repo.FindAll<WaitingGameDto>(It.IsAny<BusyGameSpecification>()))
                     .ReturnsAsync(new List<WaitingGameDto>());
        _gameRepoMock.Setup(repo => repo.FindAll<GameDetail>(It.IsAny<BusyGameSpecification>()))
                     .ReturnsAsync(new List<GameDetail> { inProgressGame });

        // Act
        var result = await _service.CheckIfUserIsBusy(userName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result);
    }

    /// <summary>
    /// Tests if the method detect that a user is not busy in any game.
    /// </summary>
    [Fact]
    public async Task CheckIfUserIsBusy_UserNotInAnyGame_ReturnsNull()
    {
        // Arrange
        var userName = "testUser";
        _gameRepoMock.Setup(repo => repo.FindAll<WaitingGameDto>(It.IsAny<BusyGameSpecification>()))
                     .ReturnsAsync(new List<WaitingGameDto>());
        _gameRepoMock.Setup(repo => repo.FindAll<GameDetail>(It.IsAny<BusyGameSpecification>()))
                     .ReturnsAsync(new List<GameDetail>());

        // Act
        var result = await _service.CheckIfUserIsBusy(userName);

        // Assert
        Assert.Null(result);
    }
}
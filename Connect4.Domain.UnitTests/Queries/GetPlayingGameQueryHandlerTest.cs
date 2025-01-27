using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using Connect4.Common.Model;

namespace Connect4.Domain.Queries.UnitTests;

/// <summary>
/// Unit tests for <see cref="GetPlayingGameQueryHandler"/>.
/// </summary>
[Trait("Category", "Unit")]
public class GetPlayingGameQueryHandlerTest
{
    private readonly Mock<GameQueryRepository> _gameRepositoryMock;
    private readonly GetPlayingGameQueryHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayingGameQueryHandlerTest"/> class.
    /// </summary>
    public GetPlayingGameQueryHandlerTest()
    {
        _gameRepositoryMock = new Mock<GameQueryRepository>() { CallBase = false };
        _handler = new GetPlayingGameQueryHandler(_gameRepositoryMock.Object);
    }

    /// <summary>
    /// Tests that the handler throws a <see cref="GameNotFoundException"/> when the game is not found.
    /// </summary>
    [Fact]
    public async Task Handle_GameNotFound_ThrowsGameNotFoundException()
    {
        // Arrange
        var query = new GetPlayingGameQuery(123, "TestUser");
        _gameRepositoryMock.Setup(repo => repo.GetOne(query.GameId))
                           .ReturnsAsync((GameDetail)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<GameNotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
        Assert.Contains("Game 123 not found.", ex.Message);
    }

    /// <summary>
    /// Tests that the handler throws a <see cref="GameNotFoundException"/> when the user is not a participant.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotParticipant_ThrowsGameNotFoundException()
    {
        // Arrange
        var query = new GetPlayingGameQuery(123, "TestUser");
        // var gameDetail = new GameDetail { GameId = 123, Host = "OtherUser", Guest = "AnotherUser" };
        var gameDetail = new GameDetail( new Connect4Game (123, "TestGame", "OtherUser", "InProgress", new ("OtherUser", "0000..."), null, "AnotherUser") );
        _gameRepositoryMock.Setup(repo => repo.GetOne(query.GameId))
                           .ReturnsAsync(gameDetail);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<GameNotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
        Assert.Contains("Game 123 not found.", ex.Message);
    }

    /// <summary>
    /// Tests that the handler returns a <see cref="GameDetail"/> when the game is found and the user is a participant.
    /// </summary>
    [Fact]
    public async Task Handle_ValidRequest_ReturnsGameDetail()
    {
        // Arrange
        var query = new GetPlayingGameQuery(123, "TestUser");
        var gameDetail = new GameDetail( new Connect4Game (123, "TestGame", "TestUser", "InProgress", new ("TestUser", "0000..."), null, "AnotherUser") );
        _gameRepositoryMock.Setup(repo => repo.GetOne(query.GameId))
                           .ReturnsAsync(gameDetail);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123, result.GameId);
        Assert.Equal("TestUser", result.Host);
    }
}
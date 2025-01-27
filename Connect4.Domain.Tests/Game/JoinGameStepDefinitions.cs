using Connect4.Common.Model;
using Connect4.Domain.Commands;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Tests.Mocks;
using Xunit;

namespace Connect4.Domain.Tests.Game;

/// <summary>
/// Step definitions for the Join Game acceptance tests.
/// </summary>
/// <remarks>
/// Contains the implementation of the Given, When, and Then steps required for testing
/// the Join Game feature in Connect4.
/// </remarks>
internal class JoinGameStepDefinitions
{
    private readonly GameRepositoryMock _gameRepository = new();
    private readonly GameQueryRepositoryMock _gameQueryRepository = new();
    private readonly PlayerStatusServiceMock _playerStatusService = new();
    private JoinGameCommand? _command;
    private int? _gameId;
    private Exception? _exception;

    #region Given

    /// <summary>
    /// Simulates a user with a valid JWT token.
    /// </summary>
    /// <param name="username">The username of the simulated user.</param>
    /// <returns>The current step definitions instance.</returns>
    internal JoinGameStepDefinitions GivenAUserWithValidJwtToken(string username)
    {
        // Simulate a logged-in user
        return this;
    }

    /// <summary>
    /// Adds a game with the specified ID and "WaitingForPlayers" status to the mock repositories.
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal JoinGameStepDefinitions AndAGameWithIdInWaitingForPlayersStatus(int gameId)
    {
        var game = new Connect4Game(
            gameId,
            "GameWaiting",
            "HostPlayer",
            "WaitingForPlayers",
            new("HostPlayer", "0000000;0000000;0000000;0000000;0000000;0000000")
        );
        _gameQueryRepository.Add(game);
        _gameRepository.Create(game);
        return this;
    }

    /// <summary>
    /// Adds a game with the specified ID that is already full (e.g., in progress or has two players).
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal JoinGameStepDefinitions AndAGameWithIdAlreadyFull(int gameId)
    {
        var game = new Connect4Game(
            gameId,
            "GameFull",
            "HostPlayer",
            "InProgress",
            new("HostPlayer", "0000000;0000000;0000000;0000000;0000000;0000000"),
            "GuestPlayer"
        );
        _gameQueryRepository.Add(game);
        _gameRepository.Create(game);
        return this;
    }

    /// <summary>
    /// Ensures that no game exists with the specified ID in the mock repositories.
    /// </summary>
    /// <param name="gameId">The ID of the non-existent game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal JoinGameStepDefinitions AndNoGameExistsWithId(int gameId)
    {
        // Do nothing, as the game does not exist in the mock repositories
        return this;
    }

    #endregion

    #region When

    /// <summary>
    /// Simulates the user sending a request to join a game with the specified ID.
    /// </summary>
    /// <param name="gameId">The ID of the game to join.</param>
    /// <returns>The current step definitions instance.</returns>
    internal JoinGameStepDefinitions WhenTheUserSendsARequestToJoinTheGameWithId(int gameId)
    {
        try
        {
            _command = new JoinGameCommand(gameId, "Marc");
            var handler = new JoinGameCommandHandler(_gameRepository, _playerStatusService, _gameQueryRepository);
            _gameId = handler.Handle(_command, default).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            _exception = ex;
        }

        return this;
    }

    #endregion

    #region Then

    /// <summary>
    /// Verifies that the game was successfully joined.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal JoinGameStepDefinitions ThenTheGameIsSuccessfullyJoined()
    {
        Assert.Null(_exception);
        Assert.NotNull(_gameId);

        var game = _gameRepository.Games.FirstOrDefault(g => g.Id == _gameId);
        Assert.NotNull(game);
        Assert.Equal("InProgress", game.Status);
        Assert.Equal("Marc", game.Guest);

        return this;
    }

    /// <summary>
    /// Verifies that a conflict error is returned with the expected message.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal JoinGameStepDefinitions ThenAConflictErrorIsReturnedWithMessage(string expectedMessage)
    {
        Assert.Null(_gameId);
        Assert.NotNull(_exception);
        Assert.IsType<GameNotFoundException>(_exception);
        Assert.Equal(expectedMessage, _exception.Message);

        return this;
    }

    /// <summary>
    /// Verifies that a "Game Not Found" error is returned with the expected message.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal JoinGameStepDefinitions ThenAGameNotFoundErrorIsReturnedWithMessage(string expectedMessage)
    {
        Assert.Null(_gameId);
        Assert.NotNull(_exception);
        Assert.IsType<GameNotFoundException>(_exception);
        Assert.Equal(expectedMessage, _exception.Message);

        return this;
    }

    #endregion
}

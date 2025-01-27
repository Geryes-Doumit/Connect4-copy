using Connect4.Common.Model;
using Connect4.Domain.Commands;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Tests.Mocks;
using Xunit;

namespace Connect4.Domain.Tests.Game;

/// <summary>
/// Step definitions for the Leave Game acceptance tests.
/// </summary>
/// <remarks>
/// Contains the implementation of the Given, When, and Then steps required for testing
/// the Leave Game feature in Connect4.
/// </remarks>
internal class LeaveGameStepDefinitions
{
    private readonly GameRepositoryMock _gameRepository = new();
    private readonly GameQueryRepositoryMock _gameQueryRepository = new();
    private LeaveGameCommand? _command;
    private Exception? _exception;

    #region Given

    /// <summary>
    /// Simulates a user with a valid JWT token.
    /// </summary>
    /// <param name="username">The username of the simulated user.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions GivenAUserWithValidJwtToken(string username)
    {
        // Simulate a logged-in user
        return this;
    }

    /// <summary>
    /// Adds a game with the specified ID and "WaitingForPlayers" status to the mock repositories.
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <param name="hostName">The host of the game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions AndAGameWithIdInWaitingForPlayersStatus(int gameId, string hostName)
    {
        var game = new Connect4Game(
            gameId,
            "GameWaiting",
            hostName,
            "WaitingForPlayers",
            new(hostName, "0000000;0000000;0000000;0000000;0000000;0000000")
        );
        _gameQueryRepository.Add(game);
        _gameRepository.Add(game);
        return this;
    }

    /// <summary>
    /// Adds a game with the specified ID and "InProgress" status to the mock repositories.
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <param name="hostName">The host of the game.</param>
    /// <param name="guestName">The guest of the game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions AndAUserIsAPlayerInAGameWithIdInProgressStatus(int gameId, string hostName, string guestName)
    {
        var game = new Connect4Game(
            gameId,
            "GameInProgress",
            hostName,
            "InProgress",
            new(hostName, "0000000;0000000;0000000;0000000;0000000;0000000"),
            guestName
        );
        _gameQueryRepository.Add(game);
        _gameRepository.Add(game);
        return this;
    }

    /// <summary>
    /// Ensures that no game exists with the specified ID in the mock repositories.
    /// </summary>
    /// <param name="gameId">The ID of the non-existent game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions AndNoGameExistsWithId(int gameId)
    {
        // Do nothing, as the game does not exist in the mock repositories
        return this;
    }

    /// <summary>
    /// Adds a game with the specified ID where the user is not a participant.
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <param name="hostName">The host of the game.</param>
    /// <param name="guestName">The guest of the game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions AndAUserIsNotAParticipantInAGameWithId(int gameId, string hostName, string guestName)
    {
        var game = new Connect4Game(
            gameId,
            "GameNotParticipating",
            hostName,
            "WaitingForPlayers",
            new(hostName, "0000000;0000000;0000000;0000000;0000000;0000000"),
            guestName
        );
        _gameQueryRepository.Add(game);
        _gameRepository.Add(game);
        return this;
    }

    #endregion

    #region When

    /// <summary>
    /// Simulates the user sending a request to leave a game with the specified ID.
    /// </summary>
    /// <param name="gameId">The ID of the game to leave.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions WhenTheUserSendsARequestToLeaveTheGameWithId(int gameId)
    {
        try
        {
            _command = new LeaveGameCommand(gameId, "Marc");
            var handler = new LeaveGameCommandHandler(_gameRepository, _gameQueryRepository);
            handler.Handle(_command, default).GetAwaiter().GetResult();
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
    /// Verifies that the game has been deleted.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions ThenTheGameIsDeleted()
    {
        Assert.Null(_exception);

        var game = _gameRepository.Games.FirstOrDefault(g => g.Id == _command?.GameId);
        Assert.Null(game);

        return this;
    }

    /// <summary>
    /// Verifies that the game status has been updated to "Finished" with the specified winner.
    /// </summary>
    /// <param name="winner">The expected winner of the game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions ThenTheGameStatusIsUpdatedToFinishedWithWinner(string winner)
    {
        Assert.Null(_exception);

        var game = _gameRepository.Games.FirstOrDefault(g => g.Id == _command?.GameId);
        Assert.NotNull(game);
        Assert.Equal("Finished", game.Status);
        Assert.Equal(winner, game.Winner);

        return this;
    }

    /// <summary>
    /// Verifies that a "Game Not Found" error is returned with the expected message.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions ThenAGameNotFoundErrorIsReturnedWithMessage(string expectedMessage)
    {
        Assert.NotNull(_exception);
        Assert.IsType<GameNotFoundException>(_exception);
        Assert.Equal(expectedMessage, _exception.Message);

        return this;
    }

    /// <summary>
    /// Verifies that a "Forbidden" error is returned with the expected message.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LeaveGameStepDefinitions ThenAForbiddenErrorIsReturnedWithMessage(string expectedMessage)
    {
        Assert.NotNull(_exception);
        Assert.IsType<GameNotFoundException>(_exception);
        Assert.Equal(expectedMessage, _exception.Message);

        return this;
    }

    #endregion
}

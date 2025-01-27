using Connect4.Common.Model;
using Connect4.Domain.Queries;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Tests.Mocks;
using Xunit;

namespace Connect4.Domain.Tests.GamesList;

/// <summary>
/// Step definitions for the "Get Finished Games" acceptance tests.
/// </summary>
/// <remarks>
/// Contains the implementation of the Given, When, and Then steps required for testing
/// the retrieval of the finished games history.
/// </remarks>
internal class FinishedGamesStepDefinitions
{
    private readonly GameQueryRepositoryMock _gameQueryRepository = new();
    private readonly PlayerStatusServiceMock _playerStatusService = new();
    private GetFinishedGamesQuery? _query;
    private IEnumerable<FinishedGameDto>? _result;
    private Exception? _exception;

    #region Given

    /// <summary>
    /// Adds finished games to the mock repository for a specific user.
    /// </summary>
    /// <param name="username">The username of the player.</param>
    /// <returns>The current step definitions instance.</returns>
    internal FinishedGamesStepDefinitions GivenAUserWithFinishedGames(string username)
    {
        _gameQueryRepository.AddGame(
            new Connect4Game(
                "Game 1",
                username,
                "Finished",
                new(username, "0000000;0000000;0000000;0000000;0000000;0000000")
            )
        );
        _gameQueryRepository.AddGame(
            new Connect4Game(
                "Game 2",
                username,
                "Finished",
                new(username, "0000000;0000000;0000000;0000000;0000000;0000000")
            )
        );
        return this;
    }

    /// <summary>
    /// Clears all games from the mock repository to simulate no finished games for the user.
    /// </summary>
    /// <param name="username">The username of the player.</param>
    /// <returns>The current step definitions instance.</returns>
    internal FinishedGamesStepDefinitions GivenAUserWithNoFinishedGames(string username)
    {
        _gameQueryRepository.ClearGames();
        return this;
    }

    /// <summary>
    /// Simulates the user being logged in.
    /// </summary>
    /// <param name="username">The username of the logged-in user.</param>
    /// <returns>The current step definitions instance.</returns>
    internal FinishedGamesStepDefinitions GivenTheUserIsLoggedIn(string username)
    {
        _query = new GetFinishedGamesQuery($"finished-{username}", 10, 0)
        {
            UserName = username
        };
        return this;
    }

    /// <summary>
    /// Simulates the user being busy in another game.
    /// </summary>
    /// <param name="gameId">The ID of the game the user is currently in.</param>
    /// <param name="status">The status of the current game (e.g., "InProgress").</param>
    /// <returns>The current step definitions instance.</returns>
    internal FinishedGamesStepDefinitions AndTheUserIsCurrentlyInAGameWithIdAndStatus(int gameId, string status)
    {
        _playerStatusService.AddGame(new Connect4Game(
            "Busy Game",
            "Marc",
            status,
            new("Marc", "0000000;0000000;0000000;0000000;0000000;0000000")
        )
        { Id = gameId });

        return this;
    }

    #endregion

    #region When

    /// <summary>
    /// Simulates the user requesting to view their finished games history.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal FinishedGamesStepDefinitions WhenTheUserRequestsGameHistory()
    {
        try
        {
            var handler = new GetFinishedGamesQueryHandler(_gameQueryRepository, _playerStatusService);
            _result = handler.Handle(_query!, default).GetAwaiter().GetResult();
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
    /// Verifies that the finished games history was returned successfully.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal FinishedGamesStepDefinitions ThenTheGameHistoryIsReturnedSuccessfully()
    {
        Assert.NotNull(_result);
        Assert.Null(_exception);
        Assert.NotEmpty(_result);

        foreach (var game in _result!)
        {
            Assert.Equal(typeof(FinishedGameDto), game.GetType());
        }

        return this;
    }

    /// <summary>
    /// Verifies that an empty finished games history was returned.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal FinishedGamesStepDefinitions ThenAnEmptyGameHistoryIsReturned()
    {
        Assert.NotNull(_result);
        Assert.Null(_exception);
        Assert.Empty(_result!);

        return this;
    }

    /// <summary>
    /// Verifies that a conflict error was returned due to the user being busy in another game.
    /// </summary>
    /// <param name="errorMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal FinishedGamesStepDefinitions ThenAConflictErrorIsReturned(string errorMessage)
    {
        Assert.Null(_result);
        Assert.NotNull(_exception);
        Assert.IsType<BusyUserDomainException>(_exception);
        Assert.Equal(errorMessage, _exception!.Message);

        return this;
    }

    #endregion
}

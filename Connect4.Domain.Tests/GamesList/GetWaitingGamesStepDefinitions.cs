using Connect4.Common.Model;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Queries;
using Connect4.Domain.Tests.Mocks;
using Xunit;

namespace Connect4.Domain.Tests.GamesList;

/// <summary>
/// Step definitions for the "Get Waiting Games" acceptance tests.
/// </summary>
/// <remarks>
/// Contains the implementation of the Given, When, and Then steps required for testing
/// the retrieval of games with the "WaitingForPlayers" status.
/// </remarks>
internal class GetWaitingGamesStepDefinitions
{
    private readonly GameQueryRepositoryMock _gameQueryRepository = new();
    private readonly PlayerStatusServiceMock _playerStatusService = new();
    private GetWaitingGamesQuery? _query;
    private IEnumerable<WaitingGameDto>? _result;
    private Exception? _exception;

    #region Given

    /// <summary>
    /// Simulates a user with a valid JWT token.
    /// </summary>
    /// <param name="username">The username of the simulated user.</param>
    /// <returns>The current step definitions instance.</returns>
    internal GetWaitingGamesStepDefinitions GivenAUserWithValidJwtToken(string username)
    {
        _query = new GetWaitingGamesQuery("waiting")
        {
            UserName = username
        };
        return this;
    }

    /// <summary>
    /// Adds games with the "WaitingForPlayers" status to the mock repository.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal GetWaitingGamesStepDefinitions AndThereAreGamesWithStatusWaitingForPlayers()
    {
        _gameQueryRepository.AddGame(new Connect4Game(
            "Game 1",
            "Marc",
            "WaitingForPlayers",
            new("Marc", "0000000;0000000;0000000;0000000;0000000;0000000")
        )
        { Id = 1 });

        _gameQueryRepository.AddGame(new Connect4Game(
            "Game 2",
            "John",
            "WaitingForPlayers",
            new("John", "0000000;0000000;0000000;0000000;0000000;0000000")
        )
        { Id = 2 });

        return this;
    }

    /// <summary>
    /// Clears all games from the mock repository to simulate no available games.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal GetWaitingGamesStepDefinitions AndThereAreNoGamesWithStatusWaitingForPlayers()
    {
        _gameQueryRepository.ClearGames();
        return this;
    }

    /// <summary>
    /// Simulates the user currently participating in another game with a specific ID and status.
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <param name="status">The status of the game (e.g., "InProgress").</param>
    /// <returns>The current step definitions instance.</returns>
    internal GetWaitingGamesStepDefinitions AndTheUserIsCurrentlyInAGameWithIdAndStatus(int gameId, string status)
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
    /// Simulates the user requesting to view the list of waiting games.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal GetWaitingGamesStepDefinitions WhenTheUserRequestsToViewTheWaitingGames()
    {
        try
        {
            var handler = new GetWaitingGamesQueryHandler(_gameQueryRepository, _playerStatusService);
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
    /// Verifies that the request to view waiting games was successful.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal GetWaitingGamesStepDefinitions ThenTheRequestIsSuccessful()
    {
        Assert.Null(_exception);
        return this;
    }

    /// <summary>
    /// Verifies that the response contains the list of waiting games.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal GetWaitingGamesStepDefinitions AndTheResponseContainsWaitingGames()
    {
        Assert.NotNull(_result);
        Assert.NotEmpty(_result);

        var games = _result.ToList();
        Assert.Equal(2, games.Count);

        Assert.Contains(games, game => game.GameName == "Game 1" && game.Host == "Marc");
        Assert.Contains(games, game => game.GameName == "Game 2" && game.Host == "John");
        return this;
    }

    /// <summary>
    /// Verifies that the response contains an empty list when no games are available.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal GetWaitingGamesStepDefinitions AndTheResponseContainsAnEmptyList()
    {
        Assert.NotNull(_result);
        Assert.Empty(_result);
        return this;
    }

    /// <summary>
    /// Verifies that a conflict error is returned when the user is already busy in another game.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal GetWaitingGamesStepDefinitions ThenAConflictErrorIsReturned(string expectedMessage)
    {
        Assert.NotNull(_exception);
        Assert.IsType<BusyUserDomainException>(_exception);
        Assert.Equal(expectedMessage, _exception.Message);
        return this;
    }

    #endregion
}

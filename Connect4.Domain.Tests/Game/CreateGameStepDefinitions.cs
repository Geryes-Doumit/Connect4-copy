using Connect4.Common.Model;
using Connect4.Domain.Commands;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Tests.Mocks;
using Xunit;

namespace Connect4.Domain.Tests.Game;

/// <summary>
/// Step definitions for the Create Game acceptance tests.
/// </summary>
/// <remarks>
/// Contains the implementation of the Given, When, and Then steps required for testing
/// the Create Game feature in Connect4.
/// </remarks>
internal class CreateGameStepDefinitions
{
    private readonly GameRepositoryMock _gameRepository = new();
    private readonly PlayerStatusServiceMock _playerStatusService = new();
    private CreateGameCommand? _command;
    private int? _gameId;
    private Exception? _exception;

    #region Given

    /// <summary>
    /// Simulates a user with a valid JWT token.
    /// </summary>
    /// <param name="username">The username of the simulated user.</param>
    /// <returns>The current step definitions instance.</returns>
    internal CreateGameStepDefinitions GivenAUserWithValidJwtToken(string username)
    {
        // Simulate a user with a valid token
        return this;
    }

    /// <summary>
    /// Sets a valid game name for the new game.
    /// </summary>
    /// <param name="gameName">The valid name of the game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal CreateGameStepDefinitions AndAValidGameName(string gameName)
    {
        _command = new CreateGameCommand(gameName, "Marc");
        return this;
    }

    /// <summary>
    /// Sets an invalid game name for the new game.
    /// </summary>
    /// <param name="gameName">The invalid name of the game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal CreateGameStepDefinitions AndAnInvalidGameName(string gameName)
    {
        _command = new CreateGameCommand(gameName, "Marc");
        return this;
    }

    /// <summary>
    /// Simulates a scenario where the user is already participating in another game.
    /// </summary>
    /// <param name="gameId">The ID of the game the user is already in.</param>
    /// <returns>The current step definitions instance.</returns>
    internal CreateGameStepDefinitions AndTheUserIsAlreadyInAGameWithId(int gameId)
    {
        _playerStatusService.AddGame(new Connect4Game(
            gameId,
            "ExistingGame",
            "Marc",
            "InProgress",
            new("Marc", "0000000;0000000;0000000;0000000;0000000;0000000")
        ));
        return this;
    }

    #endregion

    #region When

    /// <summary>
    /// Simulates the user sending a request to create a new game.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal CreateGameStepDefinitions WhenTheUserSendsARequestToCreateANewGame()
    {
        if (_command == null)
        {
            _exception = new DomainException("Invalid game name");
            return this;
        }

        try
        {
            var handler = new CreateGameCommandHandler(_gameRepository, _playerStatusService);
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
    /// Verifies that the game was successfully created with the specified status.
    /// </summary>
    /// <param name="expectedStatus">The expected status of the newly created game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal CreateGameStepDefinitions ThenTheGameIsSuccessfullyCreatedWithStatus(string expectedStatus)
    {
        Assert.Null(_exception);
        Assert.NotNull(_gameId);

        var createdGame = _gameRepository.Games.LastOrDefault();
        Assert.NotNull(createdGame);
        Assert.Equal(_gameId, createdGame.Id);
        Assert.Equal("MyFirstGame", createdGame.Name);
        Assert.Equal(expectedStatus, createdGame.Status);
        return this;
    }

    /// <summary>
    /// Verifies that a bad request error is returned with the specified message.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal CreateGameStepDefinitions ThenABadRequestErrorIsReturnedWithMessage(string expectedMessage)
    {
        Assert.Null(_gameId);
        Assert.NotNull(_exception);
        Assert.IsType<DomainException>(_exception);
        Assert.Equal(expectedMessage, _exception.Message);
        return this;
    }

    /// <summary>
    /// Verifies that a conflict error is returned with the specified message.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal CreateGameStepDefinitions ThenAConflictErrorIsReturnedWithMessage(string expectedMessage)
    {
        Assert.Null(_gameId);
        Assert.NotNull(_exception);
        Assert.IsType<BusyUserDomainException>(_exception);
        Assert.Equal(expectedMessage, _exception.Message);
        return this;
    }

    #endregion
}

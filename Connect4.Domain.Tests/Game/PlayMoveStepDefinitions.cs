using Connect4.Common.Model;
using Connect4.Domain.Commands;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Services;
using Connect4.Domain.Tests.Mocks;
using Xunit;

namespace Connect4.Domain.Tests.Game;

/// <summary>
/// Step definitions for the Play Move acceptance tests.
/// </summary>
/// <remarks>
/// Contains the implementation of the Given, When, and Then steps required for testing
/// the Play Move feature in Connect4.
/// </remarks>
internal class PlayMoveStepDefinitions
{
    private readonly GameQueryRepositoryMock _gameQueryRepository = new();
    private readonly GameRepositoryMock _gameRepository = new();
    private readonly BoardQueryRepositoryMock _boardQueryRepository = new();
    private readonly BoardRepositoryMock _boardRepository = new();
    private readonly GameServiceMock _gameService = new();

    private PlayMoveCommand? _command;
    private Exception? _exception;

    #region Given

    /// <summary>
    /// Simulates a user with a valid JWT token.
    /// </summary>
    /// <param name="username">The username of the simulated user.</param>
    /// <returns>The current step definitions instance.</returns>
    internal PlayMoveStepDefinitions GivenAUserWithValidJwtToken(string username)
    {
        // Simulate a logged-in user
        return this;
    }

    /// <summary>
    /// Sets up a game with the given ID, host, guest, and current player.
    /// </summary>
    /// <param name="gameId">The ID of the game.</param>
    /// <param name="host">The host of the game.</param>
    /// <param name="guest">The guest of the game.</param>
    /// <param name="currentPlayer">The current player of the game.</param>
    /// <returns>The current step definitions instance.</returns>
    internal PlayMoveStepDefinitions AndAUserIsInAGameWithIdInProgressStatus(int gameId, string host, string guest, string currentPlayer)
    {
        var board = new Board(gameId, currentPlayer, "0000000;0000000;0000000;0000000;0000000;0000000");
        var game = new Connect4Game(gameId, "InProgressGame", host, "InProgress", board, guest);
        _gameQueryRepository.Add(game);
        _gameRepository.Add(game);
        _boardQueryRepository.Add(board);
        _boardRepository.Add(board);
        return this;
    }

    /// <summary>
    /// Ensures it is the user's turn to play.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal PlayMoveStepDefinitions AndItIsTheUsersTurnToPlay()
    {
        // Assume it's the user's turn based on the currentPlayer in the board state
        return this;
    }

    /// <summary>
    /// Ensures it is not the user's turn to play.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal PlayMoveStepDefinitions AndItIsNotTheUsersTurnToPlay()
    {
        // Assume it's NOT the user's turn
        return this;
    }

    /// <summary>
    /// Simulates a column being full.
    /// </summary>
    /// <param name="column">The column that is full.</param>
    /// <returns>The current step definitions instance.</returns>
    internal PlayMoveStepDefinitions AndColumnIsFull(int column)
    {
        _gameService.MockFullColumn(column);
        return this;
    }

    #endregion

    #region When

    /// <summary>
    /// Simulates the user playing a move in the specified column.
    /// </summary>
    /// <param name="column">The column where the move is played.</param>
    /// <returns>The current step definitions instance.</returns>
    internal PlayMoveStepDefinitions WhenTheUserPlaysAMoveInColumn(int column)
    {
        try
        {
            _command = new PlayMoveCommand(1, "Marc", column);
            var handler = new PlayMoveCommandHandler(
                _gameQueryRepository,
                _gameRepository,
                _boardQueryRepository,
                _boardRepository,
                _gameService
            );
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
    /// Verifies that the game board is updated with the user's move.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal PlayMoveStepDefinitions ThenTheGameBoardIsUpdatedWithTheMove()
    {
        Assert.Null(_exception);

        var game = _gameRepository.Games.FirstOrDefault(g => g.Id == _command?.GameId);
        Assert.NotNull(game);

        var board = game!.Board;
        Assert.NotNull(board);

        // Validate that the move has been registered on the board
        Assert.Contains('1', board.State); // Assuming "Marc" is player 1
        return this;
    }

    /// <summary>
    /// Verifies that an error is returned when it is not the user's turn to play.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal PlayMoveStepDefinitions ThenANotYourTurnErrorIsReturnedWithMessage(string expectedMessage)
    {
        Assert.NotNull(_exception);
        Assert.IsType<NotYourTurnDomainException>(_exception);
        Assert.Equal(expectedMessage, _exception.Message);

        return this;
    }

    /// <summary>
    /// Verifies that an error is returned when attempting to play in a full column.
    /// </summary>
    /// <param name="expectedMessage">The expected error message.</param>
    /// <returns>The current step definitions instance.</returns>
    internal PlayMoveStepDefinitions ThenAColumnFullErrorIsReturnedWithMessage(string expectedMessage)
    {
        Assert.NotNull(_exception);
        Assert.IsType<DomainException>(_exception);
        Assert.Equal(expectedMessage, _exception.Message);

        return this;
    }

    #endregion
}

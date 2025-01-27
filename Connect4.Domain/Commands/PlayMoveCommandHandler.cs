using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using Connect4.Common.Model;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Connect4.Domain.Services;

namespace Connect4.Domain.Commands;

/// <summary>
/// Handler for processing <see cref="PlayMoveCommand"/> commands.
/// </summary>
/// <remarks>
/// This handler manages the logic for a user playing a move in an ongoing Connect4 game, including move validation and board state updates.
/// </remarks>
public class PlayMoveCommandHandler : IRequestHandler<PlayMoveCommand, int>
{
    private readonly GameQueryRepository _gameQueryRepository;
    private readonly GameRepository _gameRepository;
    private readonly BoardQueryRepository _boardQueryRepository;
    private readonly BoardRepository _boardRepository;
    private readonly IGameService _gameService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayMoveCommandHandler"/> class.
    /// </summary>
    /// <param name="gameQueryRepository">The repository for querying game entities.</param>
    /// <param name="gameRepository">The repository for managing game entities.</param>
    /// <param name="boardQueryRepository">The repository for querying board entities.</param>
    /// <param name="boardRepository">The repository for managing board entities.</param>
    /// <param name="gameService">The service for game-related operations.</param>
    public PlayMoveCommandHandler(
        GameQueryRepository gameQueryRepository,
        GameRepository gameRepository,
        BoardQueryRepository boardQueryRepository,
        BoardRepository boardRepository,
        IGameService gameService
    )
    {
        _gameQueryRepository = gameQueryRepository;
        _gameRepository = gameRepository;
        _boardQueryRepository = boardQueryRepository;
        _boardRepository = boardRepository;
        _gameService = gameService;
    }

    /// <summary>
    /// Handles the process of playing a move by processing the <see cref="PlayMoveCommand"/>.
    /// </summary>
    /// <param name="request">The <see cref="PlayMoveCommand"/> containing move data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The ID of the game where the move was played.</returns>
    /// <exception cref="GameNotFoundException">
    /// Thrown when the specified game does not exist or is not in a playable state.
    /// </exception>
    /// <exception cref="NotYourTurnDomainException">
    /// Thrown when it is not the user's turn to play.
    /// </exception>
    /// <exception cref="DomainException">
    /// Thrown when the move attempted by the user is invalid.
    /// </exception>
    public async Task<int> Handle(PlayMoveCommand request, CancellationToken cancellationToken)
    {
        // 1. Retrieve the game and board from the database
        var game = await _gameQueryRepository.GetOne(request.GameId);
        if (game == null || !(game.GetType() == typeof(GameDetail)))
        {
            throw new GameNotFoundException($"Game with id {request.GameId} not found.");
        }

        GameDetail _game = (GameDetail)game;

        if (_game.Host != request.Username && _game.Guest != request.Username)
        {
            throw new GameNotFoundException($"Game with id {request.GameId} not found.");
        }

        if (_game.Status != "InProgress")
        {
            throw new NotYourTurnDomainException("Waiting for the other player to join the game.");
        }

        //var _board = await _boardQueryRepository.GetOne(_game.GameId);
        var _board = _game.Board;

        if (_board.CurrentPlayer != request.Username)
        {
            throw new NotYourTurnDomainException("It is not your turn to play.");
        }

        // 2. Validate the player's move
        if(!_gameService.ValidateMove(_board, request.Column))
        {
            throw new DomainException("Invalid move.");
        }

        // 3. Update the board state
        char userColor = (_game.Host == request.Username) ? '1' : '2';
        _board.State = _gameService.UpdateBoard(_board, userColor, request.Column);

        var gameConnect4 = _game.ToConnect4Game();

        // 4. Save the updated game and board back to the database
        string nextCurrentPlayer = (_game.Host == _board.CurrentPlayer) ? _game.Guest : _game.Host;

        await _boardRepository.UpdateBoard(_game.GameId, _board.State, nextCurrentPlayer);

        // 5. Check if the game has been won
        if (_gameService.CheckWin(_board, userColor))
        {
            await _gameRepository.Finished(_game.GameId, request.Username);
        }

        if (_gameService.CheckDraw(_board))
        {
            await _gameRepository.Finished(_game.GameId, null);
        }

        return request.GameId; // Indicates success without returning additional data
    }
}

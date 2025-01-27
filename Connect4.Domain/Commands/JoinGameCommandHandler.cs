using Connect4.Common.Model;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using Connect4.Domain.Services;
using MediatR;

namespace Connect4.Domain.Commands;

/// <summary>
/// Handler for processing <see cref="JoinGameCommand"/> commands.
/// </summary>
/// <remarks>
/// This handler manages the logic for a user joining an existing Connect4 game, ensuring that the user is not already in another game
/// and that the target game is available for joining.
/// </remarks>
public class JoinGameCommandHandler : IRequestHandler<JoinGameCommand, int>
{
    private readonly GameRepository _gameRepository;
    private readonly GameQueryRepository _gameQueryRepository;
    private readonly IPlayerStatusService _playerStatus;

    /// <summary>
    /// Initializes a new instance of the <see cref="JoinGameCommandHandler"/> class.
    /// </summary>
    /// <param name="gameRepository">The repository for managing game entities.</param>
    /// <param name="playerStatus">The service for checking player status.</param>
    /// <param name="gameQueryRepository">The repository for querying game entities.</param>
    public JoinGameCommandHandler(GameRepository gameRepository, IPlayerStatusService playerStatus, GameQueryRepository gameQueryRepository)
    {
        _gameRepository = gameRepository;
        _playerStatus = playerStatus;
        _gameQueryRepository = gameQueryRepository;
    }

    /// <summary>
    /// Handles the process of joining a game by processing the <see cref="JoinGameCommand"/>.
    /// </summary>
    /// <param name="request">The <see cref="JoinGameCommand"/> containing game join data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The ID of the game that was joined.</returns>
    /// <exception cref="BusyUserDomainException">
    /// Thrown when the user is already participating in another game.
    /// </exception>
    /// <exception cref="GameNotFoundException">
    /// Thrown when the specified game does not exist or is not in a joinable state.
    /// </exception>
    public async Task<int> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        var userName = request.UserName;
        var busyGameId = await _playerStatus.CheckIfUserIsBusy(userName);
        if (busyGameId != null)
        {
            throw new BusyUserDomainException($"You are busy in game {busyGameId}, cannot join a new game.", busyGameId);
        }

        var game = await _gameQueryRepository.GetOne(request.GameId);
        if (game == null || ((GameDetail)game).Status != "WaitingForPlayers")
        {
            throw new GameNotFoundException($"Game with id {request.GameId} not found.");
        }

        await _gameRepository.InProgress(request.GameId, userName);

        return request.GameId;
    }
}

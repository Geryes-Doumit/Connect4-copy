using Connect4.Common.Model;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Connect4.Domain.Commands;

/// <summary>
/// Handler for processing <see cref="LeaveGameCommand"/> commands.
/// </summary>
/// <remarks>
/// This handler manages the logic for a user leaving an existing Connect4 game, handling game state transitions
/// based on whether the game is waiting for players or currently in progress.
/// </remarks>
public class LeaveGameCommandHandler : IRequestHandler<LeaveGameCommand, Unit>
{
    private readonly GameRepository _gameRepository;
    private readonly GameQueryRepository _gameQueryRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeaveGameCommandHandler"/> class.
    /// </summary>
    /// <param name="gameRepository">The repository for managing game entities.</param>
    /// <param name="gameQueryRepository">The repository for querying game entities.</param>
    public LeaveGameCommandHandler(GameRepository gameRepository, GameQueryRepository gameQueryRepository)
    {
        _gameRepository = gameRepository;
        _gameQueryRepository = gameQueryRepository;
    }

    /// <summary>
    /// Handles the process of leaving a game by processing the <see cref="LeaveGameCommand"/>.
    /// </summary>
    /// <param name="request">The <see cref="LeaveGameCommand"/> containing game leave data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Unit"/> indicating the completion of the operation.</returns>
    /// <exception cref="GameNotFoundException">
    /// Thrown when the specified game does not exist.
    /// </exception>
    public async Task<Unit> Handle(LeaveGameCommand request, CancellationToken cancellationToken)
    {
        var username = request.UserName;
        var game = await _gameQueryRepository.GetOne(request.GameId);
        if (game == null)
        {
            throw new GameNotFoundException($"Game with id {request.GameId} not found.");
        }

        if (((GameDetail)game).Status == "WaitingForPlayers" && ((GameDetail)game).Host == username)
        {
            await _gameRepository.Delete(request.GameId);
        }
        else if ( ((GameDetail)game).Status == "InProgress" && ( ((GameDetail)game).Guest == username || ((GameDetail)game).Host == username ) )
        {
            string winner = ((GameDetail)game).Host == username ? ((GameDetail)game).Guest : ((GameDetail)game).Host;
            await _gameRepository.Finished(request.GameId, winner);
        }
        else if (((GameDetail)game).Status == "Finished")
        {
            return Unit.Value;
        }
        else
        {
            throw new GameNotFoundException($"Game with id {request.GameId} not found.");
        }

        return Unit.Value;
    }
}

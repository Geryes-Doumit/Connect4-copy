using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using Connect4.Domain.Services;
using MediatR;

namespace Connect4.Domain.Commands;

/// <summary>
/// Handler for processing <see cref="CreateGameCommand"/> commands.
/// </summary>
/// <remarks>
/// This handler manages the creation of a new Connect4 game, ensuring that the host player is not already in another game.
/// </remarks>
public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, int>
{
    private readonly IPlayerStatusService _playerStatus;
    private readonly GameRepository _gameRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateGameCommandHandler"/> class.
    /// </summary>
    /// <param name="gameRepository">The repository for managing game entities.</param>
    /// <param name="playerStatus">The service for checking player status.</param>
    public CreateGameCommandHandler(GameRepository gameRepository, IPlayerStatusService playerStatus)
    {
        _gameRepository = gameRepository;
        _playerStatus = playerStatus;
    }

    /// <summary>
    /// Handles the creation of a new game by processing the <see cref="CreateGameCommand"/>.
    /// </summary>
    /// <param name="request">The <see cref="CreateGameCommand"/> containing game creation data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The unique identifier of the newly created game.</returns>
    /// <exception cref="BusyUserDomainException">
    /// Thrown when the host player is already participating in another game.
    /// </exception>
    public async Task<int> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var userName = request.Game.Host;
        var busyGameId = await _playerStatus.CheckIfUserIsBusy(userName);
        if (busyGameId != null)
        {
            throw new BusyUserDomainException($"You are busy in game {busyGameId}, cannot create a new game.", busyGameId);
        }

        if (request.Game.Name.Length < 3 || request.Game.Name.Length > 50)
        {
            throw new DomainException("Invalid game name");
        }

        return await _gameRepository.Create(request.Game);
    }
}

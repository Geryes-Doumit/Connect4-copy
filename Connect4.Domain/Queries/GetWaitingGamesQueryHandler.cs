using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using MediatR;
using Connect4.Common.Model;
using Connect4.Domain.Services;

namespace Connect4.Domain.Queries;

/// <summary>
/// Handler for <see cref="GetWaitingGamesQuery"/>.
/// Uses <see cref="GameQueryRepository"/> to fetch data and apply <see cref="GameListFilterSpecification"/>.
/// </summary>
/// <remarks>
/// This handler retrieves all waiting games, ensuring the user is not currently busy in another game.
/// </remarks>
public class GetWaitingGamesQueryHandler
    : IRequestHandler<GetWaitingGamesQuery, IEnumerable<WaitingGameDto>>
{
    private readonly GameQueryRepository _gameRepository;
    private readonly IPlayerStatusService _playerStatus;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetWaitingGamesQueryHandler"/> class.
    /// </summary>
    /// <param name="gameRepository">The repository for querying game entities.</param>
    /// <param name="playerStatus">The service for checking player status.</param>
    public GetWaitingGamesQueryHandler(GameQueryRepository gameRepository, IPlayerStatusService playerStatus)
    {
        _gameRepository = gameRepository;
        _playerStatus = playerStatus;
    }

    /// <summary>
    /// Handles the retrieval of waiting games by processing the <see cref="GetWaitingGamesQuery"/>.
    /// </summary>
    /// <param name="request">The <see cref="GetWaitingGamesQuery"/> containing query data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An enumerable of <see cref="WaitingGameDto"/> representing the waiting games.</returns>
    /// <exception cref="BusyUserDomainException">
    /// Thrown when the user is currently participating in another game.
    /// </exception>
    public async Task<IEnumerable<WaitingGameDto>> Handle(GetWaitingGamesQuery request, CancellationToken cancellationToken)
    {
        var userName = request.UserName;

        var busyGameId = await _playerStatus.CheckIfUserIsBusy(userName);

        if (busyGameId != null)
        {
            throw new BusyUserDomainException($"You are busy in game {busyGameId}, cannot access waiting games.", busyGameId);
        }

        var specification = new GameListFilterSpecification(request.CategoryFilter);
        return await _gameRepository.Find<WaitingGameDto>(
            request.LimitOrDefault,
            request.OffsetOrDefault,
            specification
        );
    }
}

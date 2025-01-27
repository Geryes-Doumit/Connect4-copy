using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using MediatR;
using Connect4.Common.Model;
using Connect4.Domain.Services;

namespace Connect4.Domain.Queries;

/// <summary>
/// Handler for <see cref="GetFinishedGamesQuery"/>.
/// Fetches finished games for a user, based on a "finished-<username>" filter.
/// </summary>
/// <remarks>
/// This handler retrieves all finished games in which the specified user participated, ensuring the user is not currently busy in another game.
/// </remarks>
public class GetFinishedGamesQueryHandler
    : IRequestHandler<GetFinishedGamesQuery, IEnumerable<FinishedGameDto>>
{
    private readonly GameQueryRepository _gameRepository;
    private readonly IPlayerStatusService _playerStatus;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFinishedGamesQueryHandler"/> class.
    /// </summary>
    /// <param name="gameRepository">The repository for querying game entities.</param>
    /// <param name="playerStatus">The service for checking player status.</param>
    public GetFinishedGamesQueryHandler(GameQueryRepository gameRepository, IPlayerStatusService playerStatus)
    {
        _gameRepository = gameRepository;
        _playerStatus = playerStatus;
    }

    /// <summary>
    /// Handles the retrieval of finished games by processing the <see cref="GetFinishedGamesQuery"/>.
    /// </summary>
    /// <param name="request">The <see cref="GetFinishedGamesQuery"/> containing query data.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An enumerable of <see cref="FinishedGameDto"/> representing the finished games.</returns>
    /// <exception cref="BusyUserDomainException">
    /// Thrown when the user is currently participating in another game.
    /// </exception>
    public async Task<IEnumerable<FinishedGameDto>> Handle(GetFinishedGamesQuery request, CancellationToken cancellationToken)
    {
        var userName = request.UserName;

        var busyGameId = await _playerStatus.CheckIfUserIsBusy(userName);
        if (busyGameId != null)
        {
            throw new BusyUserDomainException($"You are busy in game {busyGameId}, cannot access finished games.", busyGameId);
        }

        var specification = new GameListFilterSpecification(request.CategoryFilter);
        return await _gameRepository.Find<FinishedGameDto>(
            request.LimitOrDefault,
            request.OffsetOrDefault,
            specification
        );
    }
}

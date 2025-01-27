using Connect4.Common.Contracts;
using Connect4.Common.Model;
using Connect4.Domain.Repositories;
using System.Linq.Expressions;

namespace Connect4.Domain.Services;

/// <summary>
/// Service interface for checking the status of a player.
/// </summary>
/// <remarks>
/// Provides functionality to determine if a user is currently busy in a waiting or in-progress game.
/// </remarks>
public interface IPlayerStatusService
{
    /// <summary>
    /// Checks if the user is currently busy in a waiting or in-progress game.
    /// </summary>
    /// <param name="userName">The username of the player.</param>
    /// <returns>The ID of the blocking game if any, or null if the user is free.</returns>
    Task<int?> CheckIfUserIsBusy(string userName);
}

/// <summary>
/// Service implementation for checking the status of a player.
/// </summary>
/// <remarks>
/// This service provides functionality to determine if a user is currently busy in a waiting or in-progress game.
/// </remarks>
public class PlayerStatusService : IPlayerStatusService
{
    private readonly GameQueryRepository _gameRepo;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerStatusService"/> class.
    /// </summary>
    /// <param name="gameRepo">The repository for querying game entities.</param>
    public PlayerStatusService(GameQueryRepository gameRepo)
    {
        _gameRepo = gameRepo;
    }

    /// <summary>
    /// Checks if the user is currently busy in a waiting or in-progress game.
    /// </summary>
    /// <param name="userName">The username of the player.</param>
    /// <returns>The ID of the blocking game if any, or null if the user is free.</returns>
    public async Task<int?> CheckIfUserIsBusy(string userName)
    {
        // 1) Retrieve all games with status "WaitingForPlayers" or "InProgress" where the user is host or guest
        var spec = new BusyGameSpecification(userName);
        var waitingGames = await _gameRepo.FindAll<WaitingGameDto>(spec);
        var inProgressGames = await _gameRepo.FindAll<GameDetail>(spec);

        // 2) If there is at least one game, return its ID
        var firstWaitingGame = waitingGames.FirstOrDefault();
        var firstInProgressGame = inProgressGames.FirstOrDefault();
        if (firstWaitingGame != null || firstInProgressGame != null)
        {
            if (firstWaitingGame != null)
            {
                Console.WriteLine($"User has been found in a Waiting Game! GameId: {firstWaitingGame.GameId}");
                return firstWaitingGame.GameId;
            }
            else if (firstInProgressGame != null)
            {
                Console.WriteLine($"User has been found in an InProgress Game! GameId: {firstInProgressGame.GameId}");
                return firstInProgressGame.GameId;
            }
        }
        return null; // Not found => the player is free
    }
}



/// <summary>
/// Specification to retrieve games with status "WaitingForPlayers" or "InProgress" where the user is host or guest.
/// </summary>
/// <remarks>
/// This specification is used to filter games based on their status and participant roles to determine if a user is busy.
/// </remarks>
public class BusyGameSpecification : Specification<Connect4Game>
{
    private readonly string _userName;

    /// <summary>
    /// Initializes a new instance of the <see cref="BusyGameSpecification"/> class.
    /// </summary>
    /// <param name="userName">The username of the player to check.</param>
    public BusyGameSpecification(string userName)
        => _userName = userName.ToLower();

    /// <summary>
    /// Converts the specification to an expression for querying.
    /// </summary>
    /// <returns>An expression representing the specification criteria.</returns>
    public override Expression<Func<Connect4Game, bool>> ToExpression()
    {
        // Consider games with status "WaitingForPlayers" or "InProgress"
        // and where the user is either the host or the guest
        return game =>
            (game.Status == "WaitingForPlayers" || game.Status == "InProgress")
            && (
                game.Host.ToLower() == _userName
                || (game.Guest != null && game.Guest.ToLower() == _userName)
            );
    }
}

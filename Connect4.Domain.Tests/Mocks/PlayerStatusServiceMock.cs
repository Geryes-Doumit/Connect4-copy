using Connect4.Common.Model;
using Connect4.Domain.Services;
using System.Collections.Concurrent;

namespace Connect4.Domain.Tests.Mocks;

/// <summary>
/// Mock implementation of <see cref="IPlayerStatusService"/> for testing purposes.
/// </summary>
public class PlayerStatusServiceMock : IPlayerStatusService
{
    // Concurrent dictionary to simulate the games and their statuses
    private readonly ConcurrentDictionary<int, Connect4Game> _games = new();

    /// <summary>
    /// Adds a game to the mock service for testing.
    /// </summary>
    /// <param name="game">The game to add.</param>
    public void AddGame(Connect4Game game)
    {
        _games[game.Id] = game;
    }

    /// <summary>
    /// Clears all games from the mock service.
    /// </summary>
    public void ClearGames()
    {
        _games.Clear();
    }

    /// <summary>
    /// Checks if the user is currently busy in a waiting or in-progress game.
    /// </summary>
    /// <param name="userName">The username of the player.</param>
    /// <returns>The ID of the blocking game if any, or null if the user is free.</returns>
    public Task<int?> CheckIfUserIsBusy(string userName)
    {
        var busyGame = _games.Values
            .FirstOrDefault(game =>
                (game.Status == "WaitingForPlayers" || game.Status == "InProgress") &&
                (game.Host.Equals(userName, StringComparison.OrdinalIgnoreCase) ||
                 (game.Guest != null && game.Guest.Equals(userName, StringComparison.OrdinalIgnoreCase)))
            );

        return Task.FromResult(busyGame?.Id);
    }
}

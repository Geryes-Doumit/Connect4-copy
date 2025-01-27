using Connect4.Common.Model;
using Connect4.Domain.Repositories;

namespace Connect4.Domain.Tests.Mocks;

/// <summary>
/// Mock implementation of the <see cref="GameRepository"/> for testing purposes.
/// </summary>
/// <remarks>
/// This mock class provides an in-memory list of games to simulate CRUD operations 
/// and state transitions for Connect4 games during testing.
/// </remarks>
public class GameRepositoryMock : GameRepository
{
    private readonly List<Connect4Game> _games = new();

    /// <summary>
    /// Gets the list of games stored in the repository.
    /// </summary>
    public IReadOnlyList<Connect4Game> Games => _games;

    /// <summary>
    /// Simulates creating a new game in the repository.
    /// </summary>
    /// <param name="entity">The <see cref="Connect4Game"/> entity to create.</param>
    /// <returns>The ID of the newly created game.</returns>
    public async Task<int> Create(Connect4Game entity)
    {
        entity.Id = _games.Any() ? _games.Max(g => g.Id) + 1 : 1;
        _games.Add(entity);
        return await Task.FromResult(entity.Id);
    }

    /// <summary>
    /// Simulates updating an existing game in the repository.
    /// </summary>
    /// <param name="id">The ID of the game to update.</param>
    /// <param name="entity">The updated <see cref="Connect4Game"/> entity.</param>
    /// <exception cref="Exception">Thrown when the game with the specified ID is not found.</exception>
    public async Task Update(int id, Connect4Game entity)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game == null)
        {
            throw new Exception($"Game with ID {id} not found.");
        }
        _games.Remove(game);
        _games.Add(entity);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Simulates changing the state of a game to "WaitingForPlayers".
    /// </summary>
    /// <param name="id">The ID of the game to update.</param>
    /// <exception cref="Exception">Thrown when the game with the specified ID is not found.</exception>
    public async Task WaitingForPlayers(int id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game == null)
        {
            throw new Exception($"Game with ID {id} not found.");
        }
        game.WaitingForPlayers();
        await Task.CompletedTask;
    }

    /// <summary>
    /// Simulates changing the state of a game to "InProgress" and assigns a guest player.
    /// </summary>
    /// <param name="id">The ID of the game to update.</param>
    /// <param name="guest">The username of the guest player.</param>
    /// <exception cref="Exception">Thrown when the game with the specified ID is not found.</exception>
    public async Task InProgress(int id, string guest)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game == null)
        {
            throw new Exception($"Game with ID {id} not found.");
        }
        game.InProgress(guest);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Simulates finishing a game and assigns a winner.
    /// </summary>
    /// <param name="id">The ID of the game to update.</param>
    /// <param name="winner">The username of the winning player.</param>
    /// <exception cref="Exception">Thrown when the game with the specified ID is not found.</exception>
    public async Task Finished(int id, string winner)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game == null)
        {
            throw new Exception($"Game with ID {id} not found.");
        }
        game.Finished(winner);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Simulates deleting a game from the repository.
    /// </summary>
    /// <param name="id">The ID of the game to delete.</param>
    /// <exception cref="Exception">Thrown when the game with the specified ID is not found.</exception>
    public async Task Delete(int id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game == null)
        {
            throw new Exception($"Game with ID {id} not found.");
        }
        _games.Remove(game);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Adds a game to the repository without assigning an ID.
    /// </summary>
    /// <param name="game">The game to add.</param>
    public void Add(Connect4Game game)
    {
        _games.Add(game);
    }
}

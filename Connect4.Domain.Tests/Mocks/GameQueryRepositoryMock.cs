using Connect4.Common.Model;
using Connect4.Common.Contracts;
using Connect4.Domain.Repositories;
using Connect4.Domain.Exceptions;

namespace Connect4.Domain.Tests.Mocks;

/// <summary>
/// Mock implementation of <see cref="GameQueryRepository"/> for testing purposes.
/// </summary>
/// <remarks>
/// This class provides an in-memory list of games to simulate querying operations during testing.
/// </remarks>
public class GameQueryRepositoryMock : GameQueryRepository
{
    private readonly List<Connect4Game> _games = new();

    /// <summary>
    /// Adds a game to the mock repository.
    /// </summary>
    /// <param name="game">The <see cref="Connect4Game"/> to add.</param>
    public void AddGame(Connect4Game game)
    {
        _games.Add(game);
    }

    /// <summary>
    /// Clears all games from the mock repository.
    /// </summary>
    public void ClearGames()
    {
        _games.Clear();
    }

    /// <summary>
    /// Simulates retrieving all games that satisfy the provided specification.
    /// </summary>
    /// <typeparam name="TDto">The DTO type to return, which must derive from <see cref="Dto{Connect4Game}"/>.</typeparam>
    /// <param name="specification">The specification to filter games.</param>
    /// <returns>A collection of filtered games as DTOs.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified DTO type is not supported.</exception>
    public async Task<IEnumerable<TDto>> FindAll<TDto>(Specification<Connect4Game> specification)
        where TDto : Dto<Connect4Game>
    {
        var filteredGames = _games
            .Where(specification.IsSatisfiedBy)
            .Select(game =>
            {
                if (typeof(TDto) == typeof(WaitingGameDto)) return new WaitingGameDto(game) as TDto;
                if (typeof(TDto) == typeof(FinishedGameDto)) return new FinishedGameDto(game) as TDto;
                if (typeof(TDto) == typeof(GameDetail)) return new GameDetail(game) as TDto;
                throw new InvalidOperationException($"Unsupported DTO type: {typeof(TDto).Name}");
            })
            .Where(dto => dto != null);
        return await Task.FromResult(filteredGames);
    }

    /// <summary>
    /// Simulates retrieving a paginated list of games that satisfy the provided specification.
    /// </summary>
    /// <typeparam name="TDto">The DTO type to return, which must derive from <see cref="Dto{Connect4Game}"/>.</typeparam>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="offset">The number of results to skip before starting to collect the result set.</param>
    /// <param name="specification">The specification to filter games.</param>
    /// <returns>A paginated collection of filtered games as DTOs.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the specified DTO type is not supported.</exception>
    public async Task<IEnumerable<TDto>> Find<TDto>(int limit, int offset, Specification<Connect4Game> specification)
        where TDto : Dto<Connect4Game>
    {
        var filteredGames = _games
            .Where(specification.IsSatisfiedBy)
            .Skip(offset)
            .Take(limit)
            .Select(game =>
            {
                if (typeof(TDto) == typeof(WaitingGameDto)) return new WaitingGameDto(game) as TDto;
                if (typeof(TDto) == typeof(FinishedGameDto)) return new FinishedGameDto(game) as TDto;
                if (typeof(TDto) == typeof(GameDetail)) return new GameDetail(game) as TDto;
                throw new InvalidOperationException($"Unsupported DTO type: {typeof(TDto).Name}");
            })
            .Where(dto => dto != null);
        return await Task.FromResult(filteredGames);
    }

    /// <summary>
    /// Simulates retrieving a single game by its ID.
    /// </summary>
    /// <param name="id">The ID of the game to retrieve.</param>
    /// <returns>The game as a <see cref="Dto{Connect4Game}"/>.</returns>
    /// <exception cref="GameNotFoundException">Thrown when no game is found with the specified ID.</exception>
    public async Task<Dto<Connect4Game>> GetOne(int id)
    {
        var game = _games.FirstOrDefault(g => g.Id == id);
        if (game == null)
        {
            throw new GameNotFoundException($"Game with id {id} not found.");
        }
        return await Task.FromResult(new GameDetail(game));
    }

    /// <summary>
    /// Adds a game to the mock repository.
    /// </summary>
    /// <param name="game">The <see cref="Connect4Game"/> to add.</param>
    public void Add(Connect4Game game)
    {
        _games.Add(game);
    }
}

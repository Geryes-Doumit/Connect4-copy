using Microsoft.EntityFrameworkCore;
using Connect4.Common.Contracts;
using Connect4.Common.Model;
using Connect4.Domain.Repositories;
using Connect4.Infrastructure.Model;
using Connect4.Domain.Exceptions;

namespace Connect4.Infrastructure.Repositories;

/// <summary>
/// SQLite-based implementation of <see cref="GameQueryRepository"/>,
/// providing methods to query user entities.
/// </summary>
public class GameQueryRepositorySqlite(DatabaseContext context) : GameQueryRepository
{
    private IQueryable<GameSqlite> Query => context.Set<GameSqlite>();

    public async Task<IEnumerable<TDto>> FindAll<TDto>(Specification<Connect4Game> specification)
        where TDto : Dto<Connect4Game>
    {
        // Step 1: Query the database
        var games = await Query
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Winner)
            .Include(g => g.Board)
            .AsNoTracking()
            .ToListAsync();

        games.Reverse();

        // Step 2: Apply the specification and filter in-memory
        var filteredGames = games
            .Select(g => g.ToDomain())
            .Where(specification.IsSatisfiedBy);

        // Step 3: Map the filtered games to the appropriate DTO
        var result = filteredGames.Select(game =>
        {
            // Use runtime type checking to map to the correct DTO
            if (typeof(TDto) == typeof(WaitingGameDto))
            {
                return new WaitingGameDto(game) as TDto;
            }
            else if (typeof(TDto) == typeof(FinishedGameDto))
            {
                return new FinishedGameDto(game) as TDto;
            }
            else if (typeof(TDto) == typeof(GameDetail))
            {
                return new GameDetail(game) as TDto;
            }
            else
            {
                throw new InvalidOperationException($"Unsupported DTO type: {typeof(TDto).Name}");
            }
        });

        return result.Where(dto => dto != null).ToList()!;
        /*
         Example usage :
         var specification = new WaitingGamesSpecification();
         var waitingGames = await _gameRepository.FindAll<WaitingGameDto>(specification);

        */
    }

    /// <summary>
    /// Retrieves a set of <typeparamref name="TDto"/> objects representing users
    /// that match the given specification. Supports pagination via <paramref name="limit"/> and <paramref name="offset"/>.
    /// </summary>
    /// <typeparam name="TDto">A Data Transfer Object type that wraps or represents a <see cref="Connect4Game"/>.</typeparam>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="offset">Zero-based index specifying how many results to skip.</param>
    /// <param name="specification">A specification used to filter the games.</param>
    /// <returns>An asynchronous task that resolves to a collection of <typeparamref name="TDto"/>.</returns>
    public async Task<IEnumerable<TDto>> Find<TDto>(int limit, int offset, Specification<Connect4Game> specification)
        where TDto : Dto<Connect4Game>
    {
        var games = await Query
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Winner)
            .Include(g => g.Board)
            .AsNoTracking()
            .ToListAsync();

        games.Reverse();

        // Query the database for games matching the specification
        var filteredGames = games
            .Select(g => g.ToDomain())
            .Where(specification.IsSatisfiedBy)
            .Skip(offset)
            .Take(limit);

        // Map the filtered games to the appropriate DTO
        var result = filteredGames.Select(game =>
        {
            // Use runtime type checking to map to the correct DTO
            if (typeof(TDto) == typeof(WaitingGameDto))
            {
                return new WaitingGameDto(game) as TDto;
            }
            else if (typeof(TDto) == typeof(FinishedGameDto))
            {
                return new FinishedGameDto(game) as TDto;
            }
            else if (typeof(TDto) == typeof(GameDetail))
            {
                return new GameDetail(game) as TDto;
            }
            else
            {
                throw new InvalidOperationException($"Unsupported DTO type: {typeof(TDto).Name}");
            }
        });

        return result.Where(dto => dto != null).ToList()!;
    }

    /// <summary>
    /// Retrieves a single user by ID and returns it as a <see cref="Dto{User}"/>.
    /// </summary>
    /// <param name="id">The user ID to find.</param>
    /// <returns>
    /// An asynchronous task that resolves to a <see cref="UserDto"/> if found;
    /// otherwise, throws an exception if no user with that ID exists.
    /// </returns>
    public async Task<Dto<Connect4Game>> GetOne(int id)
    {
        // Fetch the game from the database
        var gameSqlite = await Query
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Winner)
            .Include(g => g.Board)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (gameSqlite == null)
        {
            throw new GameNotFoundException($"Game with id {id} not found.");
            //throw new Exception($"Game with ID {id} not found.");
        }

        var domainGame = gameSqlite.ToDomain();

        //return domainGame.Status switch
        //{
        //    "WaitingForPlayers" => new WaitingGameDto(domainGame),
        //    "InProgress" => new PlayingGameDto(domainGame),
        //    "Finished" => new FinishedGameDto(domainGame),
        //    _ => throw new InvalidOperationException($"Unsupported game status: {domainGame.Status}")
        //};
        return new GameDetail(domainGame);
    }
}

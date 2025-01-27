using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Connect4.Domain.Repositories;
using Connect4.Infrastructure.Model;
using Connect4.Common.Model;

namespace Connect4.Infrastructure.Repositories;

public class GameRepositorySqlite(DatabaseContext context) : GameRepository
{
    private DbSet<GameSqlite> Query => context.Set<GameSqlite>();

    private async Task<GameSqlite> FindWithNoTrackingAsync(int id)
    {
        var game = await Query
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Winner)
            .Include(g => g.Board)
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);

        context.Entry(game).State = EntityState.Detached;

        return game;
    }

    private async Task<GameSqlite> FindAsync(int id)
    {
        return await Query
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Winner)
            .Include(g => g.Board)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<GameSqlite> ApplyDomainMethodAsync(int id, Action<Connect4Game> method)
    {
        var game = await FindWithNoTrackingAsync(id);
        Console.WriteLine("Game found");
        var domainGame = game.ToDomain();
        Console.WriteLine("Game converted to domain");
        method(domainGame);
        var updatedGame = domainGame.ToSqlite(
            game.Host, 
            game.Guest, 
            game.Winner,
            game.Board);
        Console.WriteLine("Domain method applied");
        return updatedGame;
    }

    private async Task<int> Save(GameSqlite game)
    {
        try
        {
            Console.WriteLine("Saving game...");
            var isCreation = game.Id == default;

            EntityEntry<GameSqlite> entry;
            if (isCreation)
            {
                entry = await Query.AddAsync(game);
            }
            else
            {
                entry = Query.Update(game);
            }

            await context.SaveChangesAsync();
 
            return entry.Entity.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving game: " + ex.Message);
            throw; // or handle specifically
        }
    }

    private async Task<UserSqlite?> HandleUser(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        var existingUser =  await context.Set<UserSqlite>()
            .FirstOrDefaultAsync(u => u.Username == username);

        return existingUser ?? new() { Username = username, HashPwd = "" };
    }

    public async Task<int> Create(Connect4Game entity)
    {
        Console.WriteLine("Creating a game...");

        // Look up the actual UserSqlite for the host:
        var hostUser = await HandleUser(entity.Host);
        if (hostUser == null)
        {
            throw new Exception($"Host user '{entity.Host}' does not exist in the database.");
        }

        // Use the new signature for HandleBoard, passing in hostUser.Id
        //var board = await HandleBoard(entity.Board, hostUser.Id);
        var board = entity.Board.ToSqlite(hostUser);

        // Convert Domain to Persistence Model
        var game = entity.ToSqlite(
            hostUser,
            await HandleUser(entity.Guest),
            await HandleUser(entity.Winner),
            board
        );

        Console.WriteLine($"Game Name: {game.Name}, Host: {game.Host?.Username}, Board: {game.Board?.State}");

        // Save the game to the database
        var generatedId = await Save(game);
        Console.WriteLine($"Game created with ID: {generatedId}");

        // Assign the generated ID back to the domain object
        entity.Id = generatedId;

        return generatedId;
    }

    public async Task Update(int id, Connect4Game entity)
    {
        var game = await FindWithNoTrackingAsync(id);
        var hostUser = await HandleUser(entity.Host);
        var updatedGame = entity.ToSqlite(
            hostUser, 
            await HandleUser(entity.Guest), 
            await HandleUser(entity.Winner),
            //await HandleBoard(entity.Board, hostUser.Id));
            entity.Board.ToSqlite(hostUser));
        await Save(updatedGame);
    }

    public async Task WaitingForPlayers(int id)
    {
        var updatedGame = await ApplyDomainMethodAsync(id, game => game.WaitingForPlayers());
        await Save(updatedGame);
    }

    public async Task InProgress(int id, string guest)
    {
        // 1. Fetch the game in a tracked state
        var existingGame = await FindAsync(id);

        // If no record found, handle accordingly
        if (existingGame == null)
        {
            throw new Exception("Game not found.");
        }
        var domainGame = existingGame.ToDomain(); 

        // 3. Apply domain operation
        domainGame.InProgress(guest);

        // 4. Update the EF entity directly (no re-attachment)
        if (domainGame.Guest != null)
        {
            var guestUser = await context.Users
                .SingleOrDefaultAsync(u => u.Username == domainGame.Guest);

            existingGame.GuestId = guestUser?.Id;  
        }

        existingGame.Status = GameStatusSqlite.InProgress;

        // 5. Save the changes. EF Core will detect updated properties automatically.
        await context.SaveChangesAsync();
    }


    public async Task Finished(int id, string winner)
    {
        // 1. Fetch the game in a tracked state (similar to InProgress).
        var existingGame = await FindAsync(id);

        if (existingGame == null)
        {
            throw new Exception("Game not found.");
        }

        // 2. Convert to domain.
        var domainGame = existingGame.ToDomain();
        domainGame.Finished(winner);
        if (domainGame.Winner != null)
        {
            var winnerUser = await context.Users
                .SingleOrDefaultAsync(u => u.Username == domainGame.Winner);

            existingGame.WinnerId = winnerUser?.Id;  
        }
        existingGame.Status = GameStatusSqlite.Finished;

        await context.SaveChangesAsync();
    }


    public async Task Delete(int id) 
    {
        Console.WriteLine("Deleting a game...");
        // 1. Find the game's BoardId
        var boardId = await Query
            .Where(game => game.Id == id)
            .Select(game => game.BoardId)
            .FirstOrDefaultAsync();
        // 2. Delete the board
        if (boardId != 0)
        {
            await context.Set<BoardSqlite>()
                .Where(b => b.Id == boardId)
                .ExecuteDeleteAsync();
        }

        // 3. Delete the game
        await Query
            .Where(game => game.Id == id)
            .ExecuteDeleteAsync();
    }
        
}

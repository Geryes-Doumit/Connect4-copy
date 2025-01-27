using Connect4.Common.Model;
using Connect4.Common.Contracts;
using Connect4.Infrastructure.Repositories;
using Connect4.Infrastructure.Model;
using Connect4.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Connect4.Infrastructure.UnitTests;

/// <summary>
/// Unit tests for the <see cref="GameRepositorySqlite"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class GameRepositoryTests : RepositoryTest
{
    private readonly GameRepositorySqlite _repository;

    private const string _name = "testGame";
    private const string _host = "host";
    private const string _guest = "guest";
    private const string _winner = "winner";
    private const string _status = "WaitingForPlayers";
    public const string _board = "0000000;0000000;0000000;0000000;0000000;0000000";

    /// <summary>
    /// Initializes a new instance of the <see cref="GameRepositoryTests"/> class.
    /// </summary>
    public GameRepositoryTests(DatabaseContext? context = null)
    {
        if (context == null)
        {
            _repository = new(Context);
        }
        else
        {
            _repository = new(context);
        }
    }

    /// <summary>
    /// Adds a new game to the database.
    /// </summary>
    public Task<int> AddGame()
    {
        var game = new Connect4Game(
            _name,
            _host,
            _status,
            new Board(_name, _board),
            _guest,
            _winner
        );

        return _repository.Create(game);
    }

    /// <summary>
    /// Tests that a game can be created and retrieved from the database.
    /// </summary>
    [Fact]
    public async Task ShouldCreate()
    {
        var id = await AddGame();
        var game = await Context.Games.FindAsync(id);

        Assert.NotNull(game);
        Assert.Equal(_name, game.Name);
        Assert.Equal(_host, game.Host.Username);
        Assert.Equal(_guest, game.Guest.Username);
        Assert.Equal(_winner, game.Winner.Username);
        Assert.Equal(Model.GameStatusSqlite.WaitingForPlayers, game.Status);
        Assert.Equal(_board, game.Board.State);
    }

    /// <summary>
    /// Tests that a game can be updated in the database.
    /// </summary>
    [Fact]
    public async Task ShouldUpdate()
    {
        var id = await AddGame();
        var game = await Context.Games.FindAsync(id);

        game.Status = Model.GameStatusSqlite.InProgress;
        var domain = game.ToDomain();

        _repository.Update(id, domain);

        var updatedGame = await Context.Games
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Winner)
            .Include(g => g.Board)
            .FirstOrDefaultAsync(g => g.Id == id);

        Assert.NotNull(updatedGame);
        Assert.Equal(_name, updatedGame.Name);
        Assert.Equal(_host, updatedGame.Host.Username);
        Assert.Equal(_guest, updatedGame.Guest.Username);
        Assert.Equal(_winner, updatedGame.Winner.Username);
        Assert.Equal(Model.GameStatusSqlite.InProgress, updatedGame.Status);
        Assert.Equal(_board, updatedGame.Board.State);
    }

    /// <summary>
    /// Tests that a game can be set to "Finished" status.
    /// 
    [Fact]
    public async Task ShouldFinished() 
    {
        var id = await AddGame();
        var game = await Context.Games.FindAsync(id);

        _repository.Finished(id, _winner);

        var updatedGame = await Context.Games
            .Include(g => g.Host)
            .Include(g => g.Guest)
            .Include(g => g.Winner)
            .Include(g => g.Board)
            .FirstOrDefaultAsync(g => g.Id == id);

        Assert.NotNull(updatedGame);
        Assert.Equal(_name, updatedGame.Name);
        Assert.Equal(_host, updatedGame.Host.Username);
        Assert.Equal(_guest, updatedGame.Guest.Username);
        Assert.Equal(_winner, updatedGame.Winner.Username);
        Assert.Equal(Model.GameStatusSqlite.Finished, updatedGame.Status);
        Assert.Equal(_board, updatedGame.Board.State);

    }

    /// <summary>
    /// Tests that a game can be deleted from the database.
    /// </summary>
    [Fact]
    public async Task ShouldDelete()
    {
        var id = await AddGame();
        var game = await Context.Games.FindAsync(id);

        _repository.Delete(id);
        Context.ChangeTracker.Clear();

        var deletedGame = await Context.Games.FindAsync(id);

        Assert.Null(deletedGame);
    }



}

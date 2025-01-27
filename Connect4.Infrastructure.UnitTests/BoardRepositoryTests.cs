using Connect4.Common.Model;
using Connect4.Common.Contracts;
using Connect4.Infrastructure.Repositories;
using Connect4.Infrastructure.Model;
using Connect4.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Connect4.Infrastructure.UnitTests;

/// <summary>
/// Unit tests for the <see cref="BoardRepositorySqlite"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class BoardRepositoryTests : RepositoryTest
{
    private readonly BoardRepositorySqlite _repository;
    private readonly GameRepositoryTests _gameTestRepository;

    private readonly string _currentPlayer = "host";

    public const string _board = "0000000;0000000;0000000;0000000;0000000;0000000";

    /// <summary>
    /// Initializes a new instance of the <see cref="BoardRepositoryTests"/> class.
    /// </summary>
    public BoardRepositoryTests()
    {
        _repository = new(Context);
        _gameTestRepository = new GameRepositoryTests(Context);
    }

    /// <summary>
    /// Adds a new board to the database.
    /// </summary>
    private async Task<int> AddBoard()
    {
        var gameId = await _gameTestRepository.AddGame();
        var game = await Context.Games.FindAsync(gameId);
        // Return the board ID
        return game.Board.Id;
    }

    /// <summary>
    /// Modifies a random character in the board string.
    /// </summary>
    /// <param name="board"></param>
    private string ModifyBoardString(string board)
    {
        var rows = board.Split(';');
        var random = new Random();
        var rowIndex = random.Next(rows.Length);
        var columnIndex = random.Next(rows[rowIndex].Length);

        // Convert the row to a char array to modify a specific character
        var rowChars = rows[rowIndex].ToCharArray();
        rowChars[columnIndex] = rowChars[columnIndex] == '0' ? '1' : '2';

        // Update the row with the modified character
        rows[rowIndex] = new string(rowChars);

        // Join the rows back together
        return string.Join(';', rows);
    }

    /// <summary>
    /// Tests that a board can be updated in the database.
    /// </summary>
    [Fact]
    public async Task ShouldUpdateBoard()
    {
        var id = await AddBoard();
        var modifiedBoard = ModifyBoardString(_board);
        var nextPlayer = _currentPlayer == "host" ? "guest" : "host";

        await _repository.UpdateBoard(id, modifiedBoard, nextPlayer);

        var board = await Context.Boards.FindAsync(id);

        Assert.NotNull(board);
        Assert.Equal(modifiedBoard, board.State);
        Assert.Equal(nextPlayer, board.CurrentPlayer.Username);
    }

}

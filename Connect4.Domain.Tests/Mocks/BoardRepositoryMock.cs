using Connect4.Common.Model;
using Connect4.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4.Domain.Tests.Mocks;

/// <summary>
/// Mock implementation of <see cref="BoardRepository"/> for testing purposes.
/// </summary>
/// <remarks>
/// This mock simulates a repository for managing game boards in-memory during unit tests.
/// </remarks>
public class BoardRepositoryMock : BoardRepository
{
    private readonly List<Board> _boards = new();

    /// <summary>
    /// Updates the state of a game board and sets the next player's turn.
    /// </summary>
    /// <param name="id">The ID of the board to update.</param>
    /// <param name="boardState">The new serialized state of the board.</param>
    /// <param name="nextPlayer">The username of the next player.</param>
    /// <returns>A completed task.</returns>
    /// <exception cref="Exception">Thrown if the board with the specified ID is not found.</exception>
    public async Task UpdateBoard(int id, string boardState, string nextPlayer)
    {
        var board = _boards.FirstOrDefault(b => b.Id == id);
        if (board == null)
        {
            throw new Exception($"Board with ID {id} not found.");
        }
        board.State = boardState;
        board.CurrentPlayer = nextPlayer;
        await Task.CompletedTask;
    }

    /// <summary>
    /// Adds a new board to the mock repository.
    /// </summary>
    /// <param name="board">The board to add.</param>
    public void Add(Board board)
    {
        _boards.Add(board);
    }
}

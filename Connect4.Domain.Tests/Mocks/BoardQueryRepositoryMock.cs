using Connect4.Common.Model;
using Connect4.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4.Domain.Tests.Mocks;

/// <summary>
/// Mock implementation of <see cref="BoardQueryRepository"/> for testing purposes.
/// </summary>
/// <remarks>
/// This mock simulates a query repository for retrieving game boards in-memory during unit tests.
/// </remarks>
public class BoardQueryRepositoryMock : BoardQueryRepository
{
    private readonly List<Board> _boards = new();

    /// <summary>
    /// Retrieves all boards stored in the mock repository.
    /// </summary>
    /// <returns>An enumerable collection of all boards.</returns>
    public async Task<IEnumerable<Board>> GetAll()
    {
        return await Task.FromResult(_boards);
    }

    /// <summary>
    /// Retrieves a single board by its ID.
    /// </summary>
    /// <param name="id">The ID of the board to retrieve.</param>
    /// <returns>The board with the specified ID.</returns>
    /// <exception cref="Exception">Thrown if no board with the specified ID is found.</exception>
    public async Task<Board> GetOne(int id)
    {
        var board = _boards.FirstOrDefault(b => b.Id == id);
        if (board == null)
        {
            throw new Exception($"Board with ID {id} not found.");
        }
        return await Task.FromResult(board);
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

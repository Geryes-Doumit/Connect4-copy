using Connect4.Common.Model;

namespace Connect4.Domain.Repositories;

public interface BoardQueryRepository
{
    /// <summary>
    /// Retrieves all boards.
    /// </summary>
    /// <returns>A task that resolves to a collection of boards.</returns>
    Task<IEnumerable<Board>> GetAll();

    /// <summary>
    /// Retrieves a single board by its ID.
    /// </summary>
    /// <param name="id">The ID of the board to retrieve.</param>
    /// <returns>A task that resolves to the board.</returns>
    Task<Board> GetOne(int id);
}

using Microsoft.EntityFrameworkCore;
using Connect4.Common.Model;
using Connect4.Domain.Repositories;
using Connect4.Infrastructure.Model;

namespace Connect4.Infrastructure.Repositories;

public class BoardQueryRepositorySqlite : BoardQueryRepository
{
    private readonly DatabaseContext _context;

    public BoardQueryRepositorySqlite(DatabaseContext context)
    {
        _context = context;
    }

    private IQueryable<BoardSqlite> Query => _context.Set<BoardSqlite>();

    /// <summary>
    /// Retrieves all boards from the database.
    /// </summary>
    public async Task<IEnumerable<Board>> GetAll()
    {
        var boardEntities = await Query.AsNoTracking().ToListAsync();
        return boardEntities.Select(b => b.ToDomain());
    }

    /// <summary>
    /// Retrieves a single board by its ID.
    /// </summary>
    public async Task<Board> GetOne(int id)
    {
        var boardSqlite = await Query
            .Include(b => b.CurrentPlayer) // Include the navigation property
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);

        if (boardSqlite == null)
        {
            throw new Exception($"Board with ID {id} not found.");
        }

        return boardSqlite.ToDomain();
    }



}

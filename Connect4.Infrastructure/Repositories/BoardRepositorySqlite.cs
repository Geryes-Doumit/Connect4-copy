using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Connect4.Domain.Repositories;
using Connect4.Infrastructure.Model;
using Connect4.Common.Model;


namespace Connect4.Infrastructure.Repositories;

public class BoardRepositorySqlite(DatabaseContext context) : BoardRepository
{

    private DbSet<BoardSqlite> Query => context.Set<BoardSqlite>();

    private async Task<BoardSqlite> FindAsync(int id)
    {
        return await Query
            .Include(b => b.CurrentPlayer)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task UpdateBoard(int id, string board, string nextPlayer)
    {
        var boardSqlite = await FindAsync(id);

        if (boardSqlite != null)
        {
            boardSqlite.State = board;

            var currentPlayer = await context.Users
                .SingleOrDefaultAsync(p => p.Username == nextPlayer);
            
            boardSqlite.CurrentPlayer = currentPlayer;
        }

        await context.SaveChangesAsync();
        
    }

}

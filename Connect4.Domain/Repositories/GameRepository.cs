using Connect4.Common.Contracts;
using Connect4.Common.Model;

namespace Connect4.Domain.Repositories;

public interface GameRepository : IRepository<Connect4Game>
{
    Task WaitingForPlayers(int id);
    Task InProgress(int id, string guest);
    Task Finished(int id, string winner);

    //Task UpdateBoard(int id, Connect4Game entity);
}

using Connect4.Common.Contracts;
using Connect4.Common.Model;

namespace Connect4.Domain.Repositories;

public interface BoardRepository
{
    Task UpdateBoard(int id, string board, string nextPlayer);

}

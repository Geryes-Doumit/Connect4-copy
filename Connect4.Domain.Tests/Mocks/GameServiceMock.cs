using Connect4.Common.Model;
using Connect4.Domain.Services;
using System;
using System.Collections.Generic;

namespace Connect4.Domain.Tests.Mocks;

/// <summary>
/// Mock implementation of <see cref="IGameService"/> for testing purposes.
/// </summary>
public class GameServiceMock : IGameService
{
    private readonly HashSet<int> _fullColumns = new();
    private readonly Dictionary<(int, char), bool> _winConditions = new();

    /// <summary>
    /// Marks a column as full for testing purposes.
    /// </summary>
    /// <param name="column">The column index to mark as full.</param>
    public void MockFullColumn(int column)
    {
        _fullColumns.Add(column);
    }

    /// <summary>
    /// Sets a win condition for a specific board and user color.
    /// </summary>
    /// <param name="boardId">The ID of the board (or hash representation).</param>
    /// <param name="userColor">The color identifier ("1" or "2") of the user to mark as a winner.</param>
    public void MockWinCondition(int boardId, char userColor)
    {
        _winConditions[(boardId, userColor)] = true;
    }

    /// <inheritdoc/>
    public bool ValidateMove(Board board, int column)
    {
        if (_fullColumns.Contains(column))
        {
            return false;
        }

        char[][] boardArray = board.State.Split(';').Select(r => r.ToCharArray()).ToArray();
        return column >= 0 && column < 7 && boardArray[0][column] == '0';
    }

    /// <inheritdoc/>
    public string UpdateBoard(Board board, char userColor, int column)
    {
        char[][] boardArray = board.State.Split(';').Select(r => r.ToCharArray()).ToArray();

        for (int row = boardArray.Length - 1; row >= 0; row--)
        {
            if (boardArray[row][column] == '0')
            {
                boardArray[row][column] = userColor;
                break;
            }
        }

        return string.Join(";", boardArray.Select(r => new string(r)));
    }

    /// <inheritdoc/>
    public bool CheckWin(Board board, char userColor)
    {
        var boardHash = board.State.GetHashCode();
        return _winConditions.TryGetValue((boardHash, userColor), out var hasWon) && hasWon;
    }

    /// <inheritdoc/>
    public bool CheckDraw(Board board)
    {
        char[][] boardArray = board.State.Split(';').Select(r => r.ToCharArray()).ToArray();
        return boardArray.All(r => r.All(c => c != '0'));
    }
}

using Connect4.Common.Model;
using Connect4.Domain.Services;
using System.Linq;

namespace Connect4.Domain.Services
{
    /// <summary>
    /// Service interface for managing Connect4 game logic.
    /// </summary>
    /// <remarks>
    /// Provides methods to validate moves, update the game board, and check for win conditions in a Connect4 game.
    /// </remarks>
    public interface IGameService
    {
        /// <summary>
        /// Validates whether a move is valid based on the current board state and the specified column.
        /// </summary>
        /// <param name="board">The current state of the game board.</param>
        /// <param name="column">The column number where the user wants to place their token.</param>
        /// <returns><c>true</c> if the move is valid; otherwise, <c>false</c>.</returns>
        bool ValidateMove(Board board, int column);

        /// <summary>
        /// Updates the game board with the user's move.
        /// </summary>
        /// <param name="board">The current state of the game board.</param>
        /// <param name="userColor">The color identifier ("1" or "2") of the user making the move.</param>
        /// <param name="column">The column number where the user wants to place their token.</param>
        /// <returns>The updated serialized state of the game board.</returns>
        string UpdateBoard(Board board, char userColor, int column);

        /// <summary>
        /// Checks if the user has achieved a win condition based on the current board state.
        /// </summary>
        /// <param name="board">The current state of the game board.</param>
        /// <param name="userColor">The color identifier ("1" or "2") of the user to check for a win.</param>
        /// <returns><c>true</c> if the user has won; otherwise, <c>false</c>.</returns>
        bool CheckWin(Board board, char userColor);


        /// <summary>
        /// Checks if the game is a draw based on the current board state.
        /// </summary>
        /// <param name="board">The current state of the game board.</param>
        /// <returns><c>true</c> if the game is a draw; otherwise, <c>false</c>.</returns>
        bool CheckDraw(Board board);
    }

    /// <summary>
    /// Implementation of the <see cref="IGameService"/> interface for Connect4 game logic.
    /// </summary>
    /// <remarks>
    /// This class provides methods to validate player moves, update the game board, and determine win conditions.
    /// </remarks>
    public class GameService : IGameService
    {
        /// <summary>
        /// Validates whether a move is valid based on the current board state and the specified column.
        /// </summary>
        /// <param name="board">The current state of the game board.</param>
        /// <param name="column">The column number where the user wants to place their token.</param>
        /// <returns><c>true</c> if the move is valid; otherwise, <c>false</c>.</returns>
        public bool ValidateMove(Board board, int column)
        {
            // Check if the column number is within the valid range (0-6)
            if (column < 0 || column >= 7)
            {
                Console.WriteLine($"Invalid move: Column {column} is out of bounds.");
                return false;
            }

            // Check if the specified column is not full
            if (!IsColumnFree(board, column))
            {
                Console.WriteLine($"Invalid move: Column {column} is full.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the game board with the user's move.
        /// </summary>
        /// <param name="board">The current state of the game board.</param>
        /// <param name="userColor">The color identifier ("1" or "2") of the user making the move.</param>
        /// <param name="column">The column number where the user wants to place their token.</param>
        /// <returns>The updated serialized state of the game board.</returns>
        public string UpdateBoard(Board board, char userColor, int column)
        {
            // Parse the board state into a 2D array
            char[][] boardArray = board.State.Split(';').Select(r => r.ToCharArray()).ToArray();

            // Place the token in the lowest available row in the specified column
            for (int row = boardArray.Length - 1; row >= 0; row--)
            {
                if (boardArray[row][column] == '0')
                {
                    boardArray[row][column] = userColor;
                    break;
                }
            }

            // Serialize the updated board back to string format
            string updatedBoardState = string.Join(";", boardArray.Select(r => new string(r)));
            return updatedBoardState;
        }

        /// <summary>
        /// Checks if the user has achieved a win condition based on the current board state.
        /// </summary>
        /// <param name="board">The current state of the game board.</param>
        /// <param name="userColor">The color identifier ("1" or "2") of the user to check for a win.</param>
        /// <returns><c>true</c> if the user has won; otherwise, <c>false</c>.</returns>
        public bool CheckWin(Board board, char userColor)
        {
            // Parse the board state into a 2D array
            char[][] boardArray = board.State.Split(';').Select(r => r.ToCharArray()).ToArray();

            for (int row = 0; row < boardArray.Length; row++)
            {
                for (int col = 0; col < boardArray[row].Length; col++)
                {
                    if (boardArray[row][col] == userColor)
                    {
                        if (CheckHorizontal(boardArray, row, col, userColor) ||
                            CheckVertical(boardArray, row, col, userColor) ||
                            CheckDiagonal(boardArray, row, col, userColor))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the game is a draw based on the current board state.
        /// </summary>
        /// <param name="board">The current state of the game board.</param>
        /// <returns><c>true</c> if the game is a draw; otherwise, <c>false</c>.</returns>
        public bool CheckDraw(Board board)
        {
            // Parse the board state into a 2D array
            char[][] boardArray = board.State.Split(';').Select(r => r.ToCharArray()).ToArray();

            // check if the last row is full
            if (boardArray[0].All(c => c != '0'))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks for a horizontal win condition from the specified position.
        /// </summary>
        /// <param name="board">The game board as a 2D array.</param>
        /// <param name="row">The row index of the starting position.</param>
        /// <param name="col">The column index of the starting position.</param>
        /// <param name="token">The token of the player ("1" or "2").</param>
        /// <returns><c>true</c> if a horizontal win is detected; otherwise, <c>false</c>.</returns>
        private bool CheckHorizontal(char[][] board, int row, int col, char token)
        {
            int count = 0;
            for (int i = Math.Max(0, col - 3); i <= Math.Min(6, col + 3); i++)
            {
                if (board[row][i] == token)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks for a vertical win condition from the specified position.
        /// </summary>
        /// <param name="board">The game board as a 2D array.</param>
        /// <param name="row">The row index of the starting position.</param>
        /// <param name="col">The column index of the starting position.</param>
        /// <param name="token">The token of the player ("1" or "2").</param>
        /// <returns><c>true</c> if a vertical win is detected; otherwise, <c>false</c>.</returns>
        private bool CheckVertical(char[][] board, int row, int col, char token)
        {
            int count = 0;
            for (int i = Math.Max(0, row - 3); i <= Math.Min(5, row + 3); i++)
            {
                if (board[i][col] == token)
                {
                    count++;
                    if (count == 4)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks for a diagonal win condition from the specified position.
        /// </summary>
        /// <param name="board">The game board as a 2D array.</param>
        /// <param name="row">The row index of the starting position.</param>
        /// <param name="col">The column index of the starting position.</param>
        /// <param name="token">The token of the player ("1" or "2").</param>
        /// <returns><c>true</c> if a diagonal win is detected; otherwise, <c>false</c>.</returns>
        private bool CheckDiagonal(char[][] board, int row, int col, char token)
        {
            // Check diagonal from top-left to bottom-right
            int count = 0;
            for (int i = -3; i <= 3; i++)
            {
                int currentRow = row + i;
                int currentCol = col + i;
                if (currentRow >= 0 && currentRow < 6 && currentCol >= 0 && currentCol < 7)
                {
                    if (board[currentRow][currentCol] == token)
                    {
                        count++;
                        if (count == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }

            // Check diagonal from bottom-left to top-right
            count = 0;
            for (int i = -3; i <= 3; i++)
            {
                int currentRow = row - i;
                int currentCol = col + i;
                if (currentRow >= 0 && currentRow < 6 && currentCol >= 0 && currentCol < 7)
                {
                    if (board[currentRow][currentCol] == token)
                    {
                        count++;
                        if (count == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the specified column on the board is free for a move.
        /// </summary>
        /// <param name="board">The current state of the game board.</param>
        /// <param name="column">The column number to check.</param>
        /// <returns><c>true</c> if the column is free; otherwise, <c>false</c>.</returns>
        private bool IsColumnFree(Board board, int column)
        {
            // Parse the board state into a 2D array
            char[][] boardArray = board.State.Split(';').Select(r => r.ToCharArray()).ToArray();

            // Check if the topmost row in the specified column is empty
            return boardArray[0][column] == '0';
        }
    }
}

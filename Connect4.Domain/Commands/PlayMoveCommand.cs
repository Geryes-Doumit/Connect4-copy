using Connect4.Common.Model;
using MediatR;

namespace Connect4.Domain.Commands;

/// <summary>
/// Command to play a move in an existing Connect4 game.
/// </summary>
/// <remarks>
/// This command encapsulates the necessary data for a user to play a move by specifying the game ID and the column number.
/// </remarks>
/// <param name="gameId">The ID of the game where the move is to be played.</param>
/// <param name="username">The username or identifier of the player making the move.</param>
/// <param name="column">The column number where the player wants to place their piece.</param>
public class PlayMoveCommand(int gameId, string username, int column) : IRequest<int>
{
    /// <summary>
    /// Gets the ID of the game where the move is to be played.
    /// </summary>
    public int GameId { get; } = gameId;

    /// <summary>
    /// Gets the username or identifier of the player making the move.
    /// </summary>
    public string Username { get; } = username;

    /// <summary>
    /// Gets the column number where the player wants to place their piece.
    /// </summary>
    public int Column { get; } = column;
}

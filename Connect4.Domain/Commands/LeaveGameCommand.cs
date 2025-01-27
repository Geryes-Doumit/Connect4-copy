using MediatR;

namespace Connect4.Domain.Commands;

/// <summary>
/// Command to leave an existing Connect4 game.
/// </summary>
/// <remarks>
/// This command encapsulates the necessary data for a user to leave a specific game by its ID.
/// </remarks>
/// <param name="gameId">The ID of the game to leave.</param>
/// <param name="userName">The username or identifier of the user leaving the game.</param>
public class LeaveGameCommand(int gameId, string userName) : IRequest<Unit>
{
    /// <summary>
    /// Gets the ID of the game to leave.
    /// </summary>
    public int GameId { get; } = gameId;

    /// <summary>
    /// Gets the username or identifier of the user leaving the game.
    /// </summary>
    public string UserName { get; } = userName;
}

using MediatR;

namespace Connect4.Domain.Commands;

/// <summary>
/// Command to join an existing Connect4 game.
/// </summary>
/// <remarks>
/// This command encapsulates the necessary data for a user to join a specific game by its ID.
/// </remarks>
/// <param name="gameId">The ID of the game to join.</param>
/// <param name="userName">The username or identifier of the user joining the game.</param>
public class JoinGameCommand(int gameId, string userName) : IRequest<int>
{
    /// <summary>
    /// Gets the username or identifier of the user joining the game.
    /// </summary>
    public string UserName { get; init; } = userName;

    /// <summary>
    /// Gets the ID of the game to join.
    /// </summary>
    public int GameId { get; init; } = gameId;
}

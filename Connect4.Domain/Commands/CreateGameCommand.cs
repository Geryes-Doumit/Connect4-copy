using Connect4.Common.Model;
using MediatR;

namespace Connect4.Domain.Commands;

/// <summary>
/// Command to create a new Connect4 game.
/// </summary>
/// <remarks>
/// This command encapsulates the necessary data to create a new game, including the game name and the host player's username.
/// </remarks>
/// <param name="gameName">The name of the game to create.</param>
/// <param name="hostName">The username or identifier of the host player.</param>
public class CreateGameCommand(string gameName, string hostName) : IRequest<int>
{
    /// <summary>
    /// Gets the <see cref="Connect4Game"/> entity to be created.
    /// </summary>
    public Connect4Game Game { get; } = new(gameName, hostName, "WaitingForPlayers", new(hostName, "0000000;0000000;0000000;0000000;0000000;0000000"));
}

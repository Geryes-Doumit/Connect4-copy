using Connect4.Common.Contracts;

namespace Connect4.Common.Model;

/// <summary>
/// Data Transfer Object representing a game that is waiting for players in Connect4.
/// </summary>
/// <remarks>
/// Provides information about games that are currently waiting for players to join.
/// </remarks>
public class WaitingGameDto : Dto<Connect4Game>
{
    /// <summary>
    /// Gets or sets the unique identifier of the game.
    /// </summary>
    public int GameId { get; set; }

    /// <summary>
    /// Gets or sets the name of the game.
    /// </summary>
    public string GameName { get; set; }

    /// <summary>
    /// Gets or sets the username or identifier of the host player.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WaitingGameDto"/> class based on a <see cref="Connect4Game"/> entity.
    /// </summary>
    /// <param name="game">The <see cref="Connect4Game"/> entity to base this DTO on.</param>
    public WaitingGameDto(Connect4Game game) : base(game)
    {
        GameId = game.Id;
        GameName = game.Name;
        Host = game.Host;
    }
}

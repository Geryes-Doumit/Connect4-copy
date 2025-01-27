using Connect4.Common.Contracts;

namespace Connect4.Common.Model;


/// <summary>
/// Data Transfer Object representing a finished game in Connect4.
/// </summary>
/// <remarks>
/// Provides detailed information about a game that has concluded, including participants and the winner.
/// </remarks>
public class FinishedGameDto : Dto<Connect4Game>
{
    /// <summary>
    /// Gets or sets the name of the game.
    /// </summary>
    public string GameName { get; set; }

    /// <summary>
    /// Gets or sets the username or identifier of the host player.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the username or identifier of the guest player.
    /// </summary>
    public string Guest { get; set; }

    /// <summary>
    /// Gets or sets the username or identifier of the winning player.
    /// </summary>
    public string Winner { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FinishedGameDto"/> class based on a <see cref="Connect4Game"/> entity.
    /// </summary>
    /// <param name="game">The <see cref="Connect4Game"/> entity to base this DTO on.</param>
    public FinishedGameDto(Connect4Game game) : base(game)
    {
        GameName = game.Name;
        Host = game.Host;
        Guest = game.Guest ?? string.Empty;
        Winner = game.Winner ?? string.Empty;
    }
}

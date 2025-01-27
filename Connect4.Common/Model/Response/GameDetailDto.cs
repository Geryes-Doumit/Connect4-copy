using System;

namespace Connect4.Common.Model;

public class GameDetailDto
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
    /// Gets or sets the username or identifier of the guest player.
    /// </summary>
    public string? Guest { get; set; }

    /// <summary>
    /// Gets or sets the username or identifier of the winning player, if any.
    /// </summary>
    public string? Winner { get; set; }

    /// <summary>
    /// Gets or sets the current status of the game.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// The <see cref="Board"/> instance representing the current state of the game board.
    /// </summary>
    public BoardDto Board { get; set; }
}

using System;

namespace Connect4.Common.Model;

public class FinishedGamesResponseDto
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
}

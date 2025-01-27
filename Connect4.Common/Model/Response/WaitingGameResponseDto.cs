namespace Connect4.Common.Model;

public class WaitingGameResponseDto
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
}

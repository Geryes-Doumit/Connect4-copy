namespace Connect4.Common.Model;

/// <summary>
/// Data Transfer Object for responses containing a game ID.
/// </summary>
public class GameIdResponsDto
{
    /// <summary>
    /// Gets or sets the success message of the login operation.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the game ID returned upon sucessful request.
    /// </summary>
    public int GameId { get; set; }
}

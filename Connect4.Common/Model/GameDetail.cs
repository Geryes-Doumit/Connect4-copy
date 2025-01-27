using Connect4.Common.Contracts;

namespace Connect4.Common.Model;

/// <summary>
/// Data Transfer Object representing an ongoing game in Connect4.
/// </summary>
/// <remarks>
/// Provides detailed information about a game that is currently in progress, including participants and board state.
/// </remarks>
public class GameDetail : Dto<Connect4Game>
{
    /// <summary>
    /// Gets or sets the unique identifier of the game.
    /// </summary>
    public int GameId { get; set; }

    /// <summary>
    /// Gets or sets the game name.
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
    public Board Board { get; set; }

    /// <summary>
    /// Converts this DTO to a <see cref="Connect4Game"/> entity.
    /// </summary>
    /// <returns>A new instance of <see cref="Connect4Game"/> based on this DTO.</returns>
    public Connect4Game ToConnect4Game()
    {
        return new Connect4Game(
            GameId,
            "SomeName",        // or get it from DTO if you have a Name property
            Host,
            Status,
            Board,
            Guest,
            Winner
        );
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameDetail"/> class based on a <see cref="Connect4Game"/> entity.
    /// </summary>
    /// <param name="game">The <see cref="Connect4Game"/> entity to base this DTO on.</param>
    public GameDetail(Connect4Game game) : base(game)
    {
        GameId = game.Id;
        GameName = game.Name;
        Host = game.Host;
        Guest = game.Guest;
        Winner = game.Winner;
        Status = game.Status;
        Board = game.Board;
    }
}

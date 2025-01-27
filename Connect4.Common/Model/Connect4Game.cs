using Connect4.Common.Contracts;

namespace Connect4.Common.Model;

/// <summary>
/// Represents a Connect4 game entity with a host, guest, current status, and winner information.
/// </summary>
public class Connect4Game : Entity
{
    /// <summary>
    /// Gets the name of the game.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the username or identifier of the host player.
    /// </summary>
    public string Host { get; }

    /// <summary>
    /// Gets the username or identifier of the guest player.
    /// </summary>
    public string? Guest { get; private set; }

    /// <summary>
    /// Gets the username or identifier of the winning player (null if the game is not finished).
    /// </summary>
    public string? Winner { get; private set; }

    /// <summary>
    /// Gets the current status of the game (e.g., "WaitingForPlayers", "InProgress", "Finished").
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// The <see cref="Board"/> instance representing the current state of the game board.
    /// </summary>
    public Board Board { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Connect4Game"/> class for creating a new game.
    /// </summary>
    /// <param name="name">The name of the game.</param>
    /// <param name="host">The username or identifier of the host player.</param>
    /// <param name="status">The initial status of the game.</param>
    /// <param name="board">The initial state of the game board.</param>
    /// <param name="guest">The username or identifier of the guest player (optional).</param>
    /// <param name="winner">The username or identifier of the winner (optional).</param>
    public Connect4Game(
        string name,
        string host,
        string status,
        Board board,
        string? guest = null,
        string? winner = null
    ) : base(0) // Default ID, will be set later by the database
    {
        Name = name;
        Host = host;
        Guest = guest;
        Winner = winner;
        Status = status;
        Board = board;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Connect4Game"/> class for existing games retrieved from the database.
    /// </summary>
    /// <param name="id">The unique identifier for the game.</param>
    /// <param name="name">The name of the game.</param>
    /// <param name="host">The username or identifier of the host player.</param>
    /// <param name="status">The current status of the game.</param>
    /// <param name="board">The current state of the game board.</param>
    /// <param name="guest">The username or identifier of the guest player (optional).</param>
    /// <param name="winner">The username or identifier of the winner (optional).</param>
    public Connect4Game(
        int id,
        string name,
        string host,
        string status,
        Board board,
        string? guest = null,
        string? winner = null
    ) : base(id) // Set ID from the database
    {
        Name = name;
        Host = host;
        Guest = guest;
        Winner = winner;
        Status = status;
        Board = board;
    }

    /// <summary>
    /// Sets the game status to "WaitingForPlayers".
    /// </summary>
    public void WaitingForPlayers() => Status = "WaitingForPlayers";

    /// <summary>
    /// Sets the game status to "InProgress" and assigns a guest player.
    /// </summary>
    /// <param name="guest">The username or identifier of the guest player.</param>
    public void InProgress(string guest)
    {
        Status = "InProgress";
        Guest = guest;
    }

    /// <summary>
    /// Sets the game status to "Finished" and assigns the winner.
    /// </summary>
    /// <param name="winner">The username or identifier of the winning player.</param>
    public void Finished(string winner)
    {
        Status = "Finished";
        Winner = winner;
    }
}

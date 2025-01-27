using Connect4.Common.Contracts;

namespace Connect4.Common.Model;

/// <summary>
/// Represents the game board for a Connect4 game, tracking the current player and the state of the board.
/// </summary>
public class Board : Entity
{
    /// <summary>
    /// Gets or sets the username or identifier of the player whose turn it is currently.
    /// </summary>
    public string CurrentPlayer { get; set; } = null!;

    /// <summary>
    /// Gets or sets the current state of the board, typically represented in a serialized format.
    /// </summary>
    public string State { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class with the specified current player and state.
    /// </summary>
    /// <param name="currentPlayer">The username or identifier of the current player.</param>
    /// <param name="state">The serialized state of the board.</param>
    public Board(string currentPlayer, string state) : base(0) // Will be set afterwards
    {
        CurrentPlayer = currentPlayer;
        State = state;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Board"/> class with the specified ID, current player, and state.
    /// </summary>
    /// <param name="id">The unique identifier for the board.</param>
    /// <param name="currentPlayer">The username or identifier of the current player.</param>
    /// <param name="state">The serialized state of the board.</param>
    public Board(int id, string currentPlayer, string state) : base(id)
    {
        CurrentPlayer = currentPlayer;
        State = state;
    }

    /// <summary>
    /// Updates the state of the board.
    /// </summary>
    /// <param name="state">The new serialized state of the board.</param>
    public void UpdateBoard(string state)
    {
        State = state;
    }
}

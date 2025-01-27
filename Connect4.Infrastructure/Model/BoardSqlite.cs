using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect4.Infrastructure.Model;

/// <summary>
/// Represents the board entity in a Connect4 game, storing the current board state and 
/// information about whose turn it is.
/// </summary>
[Table("board")]
public class BoardSqlite
{
    /// <summary>
    /// Gets or sets the unique identifier for this board record.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the ID of the user whose turn it is to play.
    /// If null, it may indicate that no current player is defined (e.g., game not started).
    /// </summary>
    [Column("current_player")]
    public int? CurrentPlayerId { get; set; }

    /// <summary>
    /// Navigation property to the user whose turn it is.
    /// </summary>
    [ForeignKey("CurrentPlayerId")]
    public UserSqlite? CurrentPlayer { get; set; }

    /// <summary>
    /// Gets or sets the serialized board state (e.g., a string representation of the grid).
    /// </summary>
    [Column("board")]
    [Required]
    public string State { get; set; } = null!;
}

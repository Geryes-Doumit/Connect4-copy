using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect4.Infrastructure.Model;

/// <summary>
/// Represents a Connect4 game entity, including participants, status, and board association.
/// </summary>
[Table("game")]
public class GameSqlite
{
    /// <summary>
    /// Gets or sets the unique identifier for this game record.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the descriptive name of the game (e.g., a custom title).
    /// </summary>
    [Column("name")]
    [Required]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the host player.
    /// </summary>
    [Column("host_id")]
    [Required]
    public int HostId { get; set; }

    /// <summary>
    /// Navigation property for the host player.
    /// </summary>
    [ForeignKey("HostId")]
    public UserSqlite Host { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the guest player (if any).
    /// </summary>
    [Column("guest_id")]
    public int? GuestId { get; set; }

    /// <summary>
    /// Navigation property for the guest player.
    /// </summary>
    [ForeignKey("GuestId")]
    public UserSqlite? Guest { get; set; }

    /// <summary>
    /// Gets or sets the ID of the winning player (if any).
    /// </summary>
    [Column("winner")]
    public int? WinnerId { get; set; }

    /// <summary>
    /// Navigation property for the winning player.
    /// </summary>
    [ForeignKey("WinnerId")]
    public UserSqlite? Winner { get; set; }

    /// <summary>
    /// Gets or sets the ID of the game status record (e.g., "InProgress", "Finished").
    /// </summary>
    [Column("status")]
    [Required]
    public GameStatusSqlite Status { get; set; }

    /// <summary>
    /// Gets or sets the ID of the associated board.
    /// </summary>
    [Column("board")]
    [Required]
    public int BoardId { get; set; }

    /// <summary>
    /// Navigation property for the board state of this game.
    /// </summary>
    [ForeignKey("BoardId")]
    public BoardSqlite Board { get; set; } = null!;
}

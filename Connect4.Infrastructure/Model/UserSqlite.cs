using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connect4.Infrastructure.Model;

/// <summary>
/// Represents a user record in the database, storing username and hashed password.
/// </summary>
[Table("user")]
public class UserSqlite
{
    /// <summary>
    /// Gets or sets the unique identifier for this user record.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the username of this user.
    /// </summary>
    [Column("username")]
    [Required]
    public string Username { get; set; } = null!;

    /// <summary>
    /// Gets or sets the hashed password for this user.
    /// </summary>
    [Column("hashPwd")]
    [Required]
    public string HashPwd { get; set; } = null!;
}

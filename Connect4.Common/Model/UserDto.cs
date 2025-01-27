using Connect4.Common.Contracts;

namespace Connect4.Common.Model;

/// <summary>
/// A DTO for the <see cref="User"/> entity, excluding sensitive fields (e.g., password hash).
/// </summary>
/// <param name="user">The user entity to wrap.</param>
public class UserDto(User user) : Dto<User>(user)
{
    /// <summary>
    /// Gets or sets the username of this user.
    /// </summary>
    public string Username { get; set; } = user.Username;
    // Note: We intentionally do not expose HashPwd for security reasons.
}

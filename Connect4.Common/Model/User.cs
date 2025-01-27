using Connect4.Common.Contracts;

namespace Connect4.Common.Model;

/// <summary>
/// Represents a user entity within the Connect4 domain.
/// </summary>
public class User : Entity
{
    /// <summary>
    /// Gets the username of this user.
    /// </summary>
    public string Username { get; }

    /// <summary>
    /// Gets the hashed password for this user.
    /// </summary>
    public string HashPwd { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class with the specified username and password hash.
    /// </summary>
    /// <param name="id">Unique identifier for the user.</param>
    /// <param name="username">The username.</param>
    /// <param name="hashPwd">The hashed password.</param>
    public User(int id, string username, string hashPwd) : base(id)
    {
        Username = username;
        HashPwd = hashPwd;
    }
}

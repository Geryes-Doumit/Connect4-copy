using Connect4.Common.Contracts;
using Connect4.Common.Model;

namespace Connect4.Domain.Repositories;

/// <summary>
/// Defines read/query methods for <see cref="User"/> entities.
/// </summary>
public interface UserQueryRepository : IQueryRepository<User>
{
    /// <summary>
    /// Retrieves a single user by their username, if found.
    /// </summary>
    /// <param name="username">The username to match.</param>
    /// <returns>An asynchronous task that resolves to a <see cref="User"/> instance or null if no match was found.</returns>
    Task<User?> GetUserByUsername(string username);
}

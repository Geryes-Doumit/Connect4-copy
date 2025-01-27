using Microsoft.EntityFrameworkCore;
using Connect4.Common.Contracts;
using Connect4.Common.Model;
using Connect4.Domain.Repositories;
using Connect4.Infrastructure.Model;

namespace Connect4.Infrastructure.Repositories;

/// <summary>
/// SQLite-based implementation of <see cref="UserQueryRepository"/>,
/// providing methods to query user entities.
/// </summary>
public class UserQueryRepositorySqlite : UserQueryRepository
{
    private readonly DatabaseContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserQueryRepositorySqlite"/> class.
    /// </summary>
    /// <param name="context">Database context used for entity operations.</param>
    public UserQueryRepositorySqlite(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a user by username from the database and maps it to a domain <see cref="User"/> object.
    /// </summary>
    /// <param name="username">The username to look up.</param>
    /// <returns>
    /// An asynchronous task that resolves to a <see cref="User"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<User?> GetUserByUsername(string username)
    {
        // Query the database for a user with the given username
        var userSqlite = await _context.Set<UserSqlite>()
            .FirstOrDefaultAsync(u => u.Username == username);

        // Convert UserSqlite to Domain User
        return userSqlite == null
            ? null
            : new User(userSqlite.Id, userSqlite.Username, userSqlite.HashPwd);
    }

    public async Task<IEnumerable<TDto>> FindAll<TDto>(Specification<User> specification)
        where TDto : Dto<User>
    {
        // Query the database for all users
        var users = _context.Set<UserSqlite>()
            .AsNoTracking()
            .Select(u => new User(
                u.Id,
                u.Username,
                u.HashPwd)) // Map to domain User
            .Where(specification.IsSatisfiedBy) // Apply specification
            .Select(u => new UserDto(u))
            .ToList(); // Convert to UserDto

        return await Task.FromResult((IEnumerable<TDto>)users);
    }

    /// <summary>
    /// Retrieves a set of <typeparamref name="TDto"/> objects representing users
    /// that match the given specification. Supports pagination via <paramref name="limit"/> and <paramref name="offset"/>.
    /// </summary>
    /// <typeparam name="TDto">A Data Transfer Object type that wraps or represents a <see cref="User"/>.</typeparam>
    /// <param name="limit">Maximum number of results to return.</param>
    /// <param name="offset">Zero-based index specifying how many results to skip.</param>
    /// <param name="specification">A specification used to filter the users.</param>
    /// <returns>An asynchronous task that resolves to a collection of <typeparamref name="TDto"/>.</returns>
    public async Task<IEnumerable<TDto>> Find<TDto>(int limit, int offset, Specification<User> specification)
        where TDto : Dto<User>
    {
        // Query the database for users matching the specification
        var users = _context.Set<UserSqlite>()
            .AsNoTracking()
            .Select(u => new User(
                u.Id,
                u.Username,
                u.HashPwd)) // Map to domain User
            .Where(specification.IsSatisfiedBy) // Apply specification
            .Skip(offset)
            .Take(limit)
            .Select(u => new UserDto(u))
            .ToList(); // Convert to UserDto

        // Since .ToList() is synchronous, we wrap it in Task.FromResult for an async signature
        return await Task.FromResult((IEnumerable<TDto>)users);
    }

    /// <summary>
    /// Retrieves a single user by ID and returns it as a <see cref="Dto{User}"/>.
    /// </summary>
    /// <param name="id">The user ID to find.</param>
    /// <returns>
    /// An asynchronous task that resolves to a <see cref="UserDto"/> if found;
    /// otherwise, throws an exception if no user with that ID exists.
    /// </returns>
    public async Task<Dto<User>> GetOne(int id)
    {
        // Fetch the user from the database
        var userSqlite = await _context.Set<UserSqlite>()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (userSqlite == null)
        {
            throw new Exception($"User with ID {id} not found.");
        }

        // Convert UserSqlite to Domain User
        var domainUser = new User(userSqlite.Id, userSqlite.Username, userSqlite.HashPwd);

        // Return the corresponding DTO
        return new UserDto(domainUser);
    }
}

using Connect4.Common.Contracts;
using Connect4.Common.Model;
using Connect4.Domain.Repositories;
using Connect4.Domain.Services;

namespace Connect4.Domain.Tests.Mocks;

/// <summary>
/// Mock implementation of the <see cref="UserQueryRepository"/> for testing purposes.
/// </summary>
/// <remarks>
/// This mock class simulates the behavior of a user repository by using an in-memory list of users.
/// It provides predefined users and supports querying by username.
/// </remarks>
public class UserQueryRepositoryMock : UserQueryRepository
{
    private readonly List<User> _users;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserQueryRepositoryMock"/> class.
    /// </summary>
    public UserQueryRepositoryMock()
    {
        var passwordService = new PasswordService();
        _users = new List<User>
        {
            new User(1, "Marc", passwordService.HashPassword("marc")),
            new User(2, "Alice", passwordService.HashPassword("alice")),
            new User(3, "Bob", passwordService.HashPassword("bob"))
        };
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username of the user to retrieve.</param>
    /// <returns>A <see cref="User"/> object if found, otherwise <c>null</c>.</returns>
    public Task<User?> GetUserByUsername(string username)
    {
        var user = _users.FirstOrDefault(u => u.Username == username);
        return Task.FromResult(user);
    }

    /// <summary>
    /// Simulates the retrieval of users based on a specification.
    /// </summary>
    /// <typeparam name="TDto">The type of DTO to return.</typeparam>
    /// <param name="specification">The specification used for filtering users.</param>
    /// <returns>An exception because this method is not needed for these tests.</returns>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public Task<IEnumerable<TDto>> FindAll<TDto>(Specification<User> specification) where TDto : Dto<User>
    {
        throw new NotImplementedException("Not needed for these tests");
    }

    /// <summary>
    /// Simulates the retrieval of a paginated list of users based on a specification.
    /// </summary>
    /// <typeparam name="TDto">The type of DTO to return.</typeparam>
    /// <param name="limit">The maximum number of results to return.</param>
    /// <param name="offset">The number of results to skip before starting to collect the result set.</param>
    /// <param name="specification">The specification used for filtering users.</param>
    /// <returns>An exception because this method is not needed for these tests.</returns>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    public Task<IEnumerable<TDto>> Find<TDto>(int limit, int offset, Specification<User> specification) where TDto : Dto<User>
    {
        throw new NotImplementedException("Not needed for these tests");
    }

    /// <summary>
    /// Simulates the retrieval of a single user by ID.
    /// </summary>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <returns>An exception because this method is not needed for these tests.</returns>
    /// <exception cref="NotImplementedException">Thrown because this method is not implemented.</exception>
    Task<Dto<User>> IQueryRepository<User>.GetOne(int id)
    {
        throw new NotImplementedException("Not needed for these tests");
    }
}

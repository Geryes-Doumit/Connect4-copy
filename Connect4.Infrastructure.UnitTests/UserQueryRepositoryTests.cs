using Connect4.Common.Model;
using Connect4.Common.Contracts;
using Connect4.Infrastructure.Repositories;
using Connect4.Infrastructure.Model;
using Connect4.Domain.Repositories;

namespace Connect4.Infrastructure.UnitTests;

/// <summary>
/// Unit tests for the <see cref="UserQueryRepositorySqlite"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class UserQueryRepositoryTests : RepositoryTest
{
    private readonly UserQueryRepositorySqlite _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserQueryRepositoryTests"/> class.
    /// </summary>
    public UserQueryRepositoryTests()
    {
        _repository = new(Context);
    }

    private const string _username = "testUser";
    private const string _hashPwd = "testHashPwd";

    /// <summary>
    /// Method to add a user to the database.
    /// </summary>
    private async Task<UserSqlite> AddUser()
    {
        var user = new UserSqlite 
        {
            Username = _username,
            HashPwd = _hashPwd
        };

        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();

        return user;
    }

    /// <summary>
    /// Test method for the <see cref="UserQueryRepositorySqlite.GetUserByUsername"/> method.
    /// </summary>
    [Fact]
    public async Task ShouldGetUserByUsername()
    {
        var user = await AddUser();
        var result = await _repository.GetUserByUsername(user.Username);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(_username, result.Username);
    }

    /// <summary>
    /// Test method for the <see cref="UserQueryRepositorySqlite.FindAll"/> method.
    /// </summary>
    [Fact]
    public async Task ShouldFind()
    {
        // Add new users with the username "testUser"
        var titiUsername = (await AddUser()).Username;
        var totoUsername = (await AddUser()).Username;
        var tata = await AddUser();

        // Query
        var results = await _repository.Find<UserDto>(10, 0, new AllSpecificationUser());

        // Filter results by username "testUser"
        var testUserResults = results.Where(r => r.Username == "testUser");

        // Assertions
        Assert.Equal(3, testUserResults.Count());
        Assert.Equal(titiUsername, testUserResults.First().Username);
        Assert.Equal(tata.Username, testUserResults.Last().Username);
    }

    /// <summary>
    /// Test method for the <see cref="UserQueryRepositorySqlite.GetOne"/> method.
    /// </summary>
    [Fact]
    public async Task ShouldGetOne()
    {
        var user = await AddUser();
        var result = (UserDto) await _repository.GetOne(user.Id);

        Assert.NotNull(result);
        Assert.Equal(user.Username, result.Username);
    }



}

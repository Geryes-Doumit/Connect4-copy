using Xunit;
using Connect4.Domain.Queries;

namespace Connect4.Domain.Queries.UnitTests;

/// <summary>
/// Unit tests for the <see cref="GetFinishedGamesQuery"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class GetFinishedGamesQueryTest
{
    /// <summary>
    /// Tests the constructor of the <see cref="GetFinishedGamesQuery"/> class.
    /// </summary>
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var categoryFilter = "finished-user";
        var limit = 5;
        var offset = 2;
        var userName = "TestUser";

        // Act
        var query = new GetFinishedGamesQuery(categoryFilter, limit, offset)
        {
            UserName = userName
        };

        // Assert
        Assert.Equal(categoryFilter, query.CategoryFilter);
        Assert.Equal(limit, query.Limit);
        Assert.Equal(offset, query.Offset);
        Assert.Equal(userName, query.UserName);
    }

    /// <summary>
    /// Tests the <see cref="GetFinishedGamesQuery.LimitOrDefault"/> property.
    /// </summary>
    [Fact]
    public void LimitOrDefault_Should_Return_10_When_Limit_Is_Null()
    {
        // Arrange
        var query = new GetFinishedGamesQuery("finished-user", null, 2);

        // Act
        var actual = query.LimitOrDefault;

        // Assert
        Assert.Equal(10, actual);
    }

    /// <summary>
    /// Tests the <see cref="GetFinishedGamesQuery.OffsetOrDefault"/> property.
    /// </summary>
    [Fact]
    public void OffsetOrDefault_Should_Return_0_When_Offset_Is_Null()
    {
        // Arrange
        var query = new GetFinishedGamesQuery("finished-user", 3, null);

        // Act
        var actual = query.OffsetOrDefault;

        // Assert
        Assert.Equal(0, actual);
    }

    /// <summary>
    /// Tests the <see cref="GetFinishedGamesQuery.UserName"/> property.
    /// </summary>
    [Fact]
    public void UserName_Property_Should_Set_And_Get_Correctly()
    {
        // Arrange
        var query = new GetFinishedGamesQuery("finished-user", 3, 5);

        // Act
        query = query with { UserName = "TestUserName" };

        // Assert
        Assert.Equal("TestUserName", query.UserName);
    }
}
using Xunit;

namespace Connect4.Domain.Queries.UnitTests;

/// <summary>
/// Unit tests for <see cref="GetWaitingGamesQuery"/>.
/// </summary>
[Trait("Category", "Unit")]
public class GetWaitingGamesQueryTest
{
    /// <summary>
    /// Tests the constructor of <see cref="GetWaitingGamesQuery"/>.
    /// </summary>
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        var categoryFilter = "waiting";
        int? limit = 5;
        int? offset = 2;
        string userName = "TestUser";

        // Act
        var query = new GetWaitingGamesQuery(categoryFilter, limit, offset)
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
    /// Tests the <see cref="GetWaitingGamesQuery.LimitOrDefault"/> property.
    /// </summary>
    [Fact]
    public void LimitOrDefault_Should_Return_10_When_Limit_Is_Null()
    {
        // Arrange
        var query = new GetWaitingGamesQuery("waiting", null, 2);

        // Act
        var actual = query.LimitOrDefault;

        // Assert
        Assert.Equal(10, actual);
    }

    /// <summary>
    /// Tests the <see cref="GetWaitingGamesQuery.OffsetOrDefault"/> property.
    /// </summary>
    [Fact]
    public void OffsetOrDefault_Should_Return_0_When_Offset_Is_Null()
    {
        // Arrange
        var query = new GetWaitingGamesQuery("waiting", 3, null);

        // Act
        var actual = query.OffsetOrDefault;

        // Assert
        Assert.Equal(0, actual);
    }

    /// <summary>
    /// Tests the <see cref="GetWaitingGamesQuery.UserName"/> property.
    /// </summary>
    [Fact]
    public void UserName_Property_Should_Set_And_Get_Correctly()
    {
        // Arrange
        var query = new GetWaitingGamesQuery("waiting", 3, 5);

        // Act
        query = query with { UserName = "TestUserName" };

        // Assert
        Assert.Equal("TestUserName", query.UserName);
    }
}
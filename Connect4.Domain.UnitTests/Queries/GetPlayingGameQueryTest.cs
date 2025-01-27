using Xunit;

namespace Connect4.Domain.Queries.UnitTests;

/// <summary>
/// Unit tests for the <see cref="GetPlayingGameQuery"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class GetPlayingGameQueryTest
{
    /// <summary>
    /// Tests that the constructor sets the properties correctly.
    /// </summary>
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        // Arrange
        int gameId = 123;
        string userName = "TestUser";

        // Act
        var query = new GetPlayingGameQuery(gameId, userName);

        // Assert
        Assert.Equal(gameId, query.GameId);
        Assert.Equal(userName, query.UserName);
    }

    /// <summary>
    /// Tests that the <see cref="GetPlayingGameQuery.GameId"/> property returns the correct value.
    /// </summary>
    [Fact]
    public void GameId_Property_Should_Return_Correct_Value()
    {
        // Arrange
        int gameId = 123;
        var query = new GetPlayingGameQuery(gameId, "TestUser");

        // Act
        var result = query.GameId;

        // Assert
        Assert.Equal(gameId, result);
    }

    /// <summary>
    /// Tests that the <see cref="GetPlayingGameQuery.UserName"/> property returns the correct value.
    /// </summary>
    [Fact]
    public void UserName_Property_Should_Return_Correct_Value()
    {
        // Arrange
        string userName = "TestUser";
        var query = new GetPlayingGameQuery(123, userName);

        // Act
        var result = query.UserName;

        // Assert
        Assert.Equal(userName, result);
    }
}
using Xunit;

namespace Connect4.Domain.Tests.GamesList;

/// <summary>
/// Acceptance tests for viewing the finished games history.
/// </summary>
[Trait("Category", "Acceptance")]
public class FinishedGamesTests
{
    private readonly FinishedGamesStepDefinitions _steps = new();

    /// <summary>
    /// Test case for successfully retrieving the history of finished games.
    /// </summary>
    [Fact]
    public void SuccessfullyViewGameHistory()
    {
        _steps.GivenAUserWithFinishedGames("Marc")
              .GivenTheUserIsLoggedIn("Marc")
              .WhenTheUserRequestsGameHistory()
              .ThenTheGameHistoryIsReturnedSuccessfully();
    }

    /// <summary>
    /// Test case for viewing finished games history when no finished games are available.
    /// </summary>
    [Fact]
    public void ViewGameHistoryWithNoFinishedGames()
    {
        _steps.GivenAUserWithNoFinishedGames("Marc")
              .GivenTheUserIsLoggedIn("Marc")
              .WhenTheUserRequestsGameHistory()
              .ThenAnEmptyGameHistoryIsReturned();
    }

    /// <summary>
    /// Test case for attempting to view finished games history while the user is busy in another game.
    /// </summary>
    [Fact]
    public void ViewGameHistoryWhileBusyInAGame()
    {
        _steps.GivenTheUserIsLoggedIn("Marc")
              .AndTheUserIsCurrentlyInAGameWithIdAndStatus(1, "InProgress")
              .WhenTheUserRequestsGameHistory()
              .ThenAConflictErrorIsReturned("You are busy in game 1, cannot access finished games.");
    }
}

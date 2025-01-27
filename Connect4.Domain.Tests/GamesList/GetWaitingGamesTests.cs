using Xunit;

namespace Connect4.Domain.Tests.GamesList;

/// <summary>
/// Acceptance tests for viewing the list of games with the "WaitingForPlayers" status.
/// </summary>
[Trait("Category", "Acceptance")]
public class GetWaitingGamesTests
{
    private readonly GetWaitingGamesStepDefinitions _steps = new();

    /// <summary>
    /// Test case for successfully viewing games with the "WaitingForPlayers" status.
    /// </summary>
    [Fact]
    public void SuccessfullyViewWaitingGames()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndThereAreGamesWithStatusWaitingForPlayers()
              .WhenTheUserRequestsToViewTheWaitingGames()
              .ThenTheRequestIsSuccessful()
              .AndTheResponseContainsWaitingGames();
    }

    /// <summary>
    /// Test case for viewing the list of games when no games are available with the "WaitingForPlayers" status.
    /// </summary>
    [Fact]
    public void ViewWaitingGamesWhenNoGamesAreAvailable()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndThereAreNoGamesWithStatusWaitingForPlayers()
              .WhenTheUserRequestsToViewTheWaitingGames()
              .ThenTheRequestIsSuccessful()
              .AndTheResponseContainsAnEmptyList();
    }

    /// <summary>
    /// Test case for attempting to view games while the user is already busy in another game.
    /// </summary>
    [Fact]
    public void ViewWaitingGamesWhileBusyInAGame()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndTheUserIsCurrentlyInAGameWithIdAndStatus(1, "InProgress")
              .WhenTheUserRequestsToViewTheWaitingGames()
              .ThenAConflictErrorIsReturned("You are busy in game 1, cannot access waiting games.");
    }
}

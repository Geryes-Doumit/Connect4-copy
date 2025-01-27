using Xunit;
using Connect4.Domain.Tests.Game;

namespace Connect4.Domain.Tests.Game;

/// <summary>
/// Acceptance tests for the Leave Game feature in a Connect4 game.
/// </summary>
[Trait("Category", "Acceptance")]
public class LeaveGameTests
{
    private readonly LeaveGameStepDefinitions _steps = new();

    /// <summary>
    /// Test case for successfully leaving a game with "WaitingForPlayers" status.
    /// </summary>
    [Fact]
    public void SuccessfullyLeaveAWaitingGame()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAGameWithIdInWaitingForPlayersStatus(1, "Marc")
              .WhenTheUserSendsARequestToLeaveTheGameWithId(1)
              .ThenTheGameIsDeleted();
    }

    /// <summary>
    /// Test case for successfully leaving a game in progress, where the other player is declared the winner.
    /// </summary>
    [Fact]
    public void SuccessfullyLeaveAnInProgressGame()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAUserIsAPlayerInAGameWithIdInProgressStatus(2, "Marc", "John")
              .WhenTheUserSendsARequestToLeaveTheGameWithId(2)
              .ThenTheGameStatusIsUpdatedToFinishedWithWinner("John");
    }

    /// <summary>
    /// Test case for attempting to leave a game that does not exist.
    /// </summary>
    [Fact]
    public void LeaveAGameThatDoesNotExist()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndNoGameExistsWithId(99)
              .WhenTheUserSendsARequestToLeaveTheGameWithId(99)
              .ThenAGameNotFoundErrorIsReturnedWithMessage("Game with id 99 not found.");
    }

    /// <summary>
    /// Test case for attempting to leave a game as a non-participant.
    /// </summary>
    [Fact]
    public void LeaveAGameAsANonParticipant()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAUserIsNotAParticipantInAGameWithId(1, "HostPlayer", "GuestPlayer")
              .WhenTheUserSendsARequestToLeaveTheGameWithId(1)
              .ThenAForbiddenErrorIsReturnedWithMessage("Game with id 1 not found.");
    }
}

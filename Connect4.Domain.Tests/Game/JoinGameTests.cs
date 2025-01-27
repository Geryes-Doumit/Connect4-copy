using Xunit;

namespace Connect4.Domain.Tests.Game;

/// <summary>
/// Acceptance tests for the Join Game feature in a Connect4 game.
/// </summary>
[Trait("Category", "Acceptance")]
public class JoinGameTests
{
    private readonly JoinGameStepDefinitions _steps = new();

    /// <summary>
    /// Test case for successfully joining a game with "WaitingForPlayers" status.
    /// </summary>
    [Fact]
    public void SuccessfullyJoinAGame()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAGameWithIdInWaitingForPlayersStatus(1)
              .WhenTheUserSendsARequestToJoinTheGameWithId(1)
              .ThenTheGameIsSuccessfullyJoined();
    }

    /// <summary>
    /// Test case for attempting to join a game that is already full.
    /// </summary>
    [Fact]
    public void JoinAGameThatIsAlreadyFull()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAGameWithIdAlreadyFull(1)
              .WhenTheUserSendsARequestToJoinTheGameWithId(1)
              .ThenAConflictErrorIsReturnedWithMessage("Game with id 1 not found.");
    }

    /// <summary>
    /// Test case for attempting to join a game that does not exist.
    /// </summary>
    [Fact]
    public void JoinAGameThatDoesNotExist()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndNoGameExistsWithId(99)
              .WhenTheUserSendsARequestToJoinTheGameWithId(99)
              .ThenAGameNotFoundErrorIsReturnedWithMessage("Game with id 99 not found.");
    }
}

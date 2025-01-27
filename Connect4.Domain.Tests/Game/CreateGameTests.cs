using Xunit;

namespace Connect4.Domain.Tests.Game;

/// <summary>
/// Acceptance tests for the Create Game feature in Connect4.
/// </summary>
[Trait("Category", "Acceptance")]
public class CreateGameTests
{
    private readonly CreateGameStepDefinitions _steps = new();

    /// <summary>
    /// Test case for successfully creating a game.
    /// </summary>
    [Fact]
    public void SuccessfullyCreateGame()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAValidGameName("MyFirstGame")
              .WhenTheUserSendsARequestToCreateANewGame()
              .ThenTheGameIsSuccessfullyCreatedWithStatus("WaitingForPlayers");
    }

    /// <summary>
    /// Test case for attempting to create a game with an invalid name.
    /// </summary>
    [Fact]
    public void CreateGameWithInvalidName()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAnInvalidGameName("")
              .WhenTheUserSendsARequestToCreateANewGame()
              .ThenABadRequestErrorIsReturnedWithMessage("Invalid game name");
    }

    /// <summary>
    /// Test case for attempting to create a game while already participating in another game.
    /// </summary>
    [Fact]
    public void CreateGameWhileAlreadyInAnotherGame()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndTheUserIsAlreadyInAGameWithId(1)
              .AndAValidGameName("NewGameWhileBusy")
              .WhenTheUserSendsARequestToCreateANewGame()
              .ThenAConflictErrorIsReturnedWithMessage("You are busy in game 1, cannot create a new game.");
    }
}

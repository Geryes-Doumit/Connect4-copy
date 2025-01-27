using Xunit;
using Connect4.Domain.Tests.Game;

namespace Connect4.Domain.Tests.Game;

/// <summary>
/// Acceptance tests for the Play Move feature in a Connect4 game.
/// </summary>
[Trait("Category", "Acceptance")]
public class PlayMoveTests
{
    private readonly PlayMoveStepDefinitions _steps = new();

    /// <summary>
    /// Test case for successfully playing a move in a game.
    /// </summary>
    [Fact]
    public void SuccessfullyPlayAMove()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAUserIsInAGameWithIdInProgressStatus(1, "Marc", "John", "Marc")
              .AndItIsTheUsersTurnToPlay()
              .WhenTheUserPlaysAMoveInColumn(3)
              .ThenTheGameBoardIsUpdatedWithTheMove();
    }

    /// <summary>
    /// Test case for attempting to play a move when it is not the user's turn.
    /// </summary>
    [Fact]
    public void PlayAMoveWhenItIsNotUsersTurn()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAUserIsInAGameWithIdInProgressStatus(1, "Marc", "John", "John")
              .AndItIsNotTheUsersTurnToPlay()
              .WhenTheUserPlaysAMoveInColumn(3)
              .ThenANotYourTurnErrorIsReturnedWithMessage("It is not your turn to play.");
    }

    /// <summary>
    /// Test case for attempting to play a move in a column that is already full.
    /// </summary>
    [Fact]
    public void PlayAMoveInAFullColumn()
    {
        _steps.GivenAUserWithValidJwtToken("Marc")
              .AndAUserIsInAGameWithIdInProgressStatus(1, "Marc", "John", "Marc")
              .AndColumnIsFull(3)
              .WhenTheUserPlaysAMoveInColumn(3)
              .ThenAColumnFullErrorIsReturnedWithMessage("Invalid move.");
    }
}

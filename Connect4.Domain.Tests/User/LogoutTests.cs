using Xunit;

namespace Connect4.Domain.Tests.Users;

/// <summary>
/// Acceptance tests for the user logout functionality.
/// </summary>
[Trait("Category", "Acceptance")]
public class LogoutTests
{
    private readonly LogoutStepDefinitions _steps = new();

    /// <summary>
    /// Test case for successfully logging out a user with a valid token.
    /// </summary>
    [Fact]
    public void SuccessfullyLogOut()
    {
        _steps.GivenAUserWithValidToken("valid-jti", DateTime.UtcNow.AddMinutes(30))
              .WhenTheUserLogsOut()
              .ThenTheLogoutIsSuccessful();
    }

    /// <summary>
    /// Test case for attempting to log out a user without a valid token.
    /// </summary>
    [Fact]
    public void LogoutWithoutValidToken()
    {
        _steps.GivenAUserWithoutToken()
              .WhenTheUserLogsOut()
              .ThenAnUnauthorizedErrorIsReturned();
    }
}

using Xunit;

namespace Connect4.Domain.Tests.Users;

/// <summary>
/// Acceptance tests for the user login functionality.
/// </summary>
/// <remarks>
/// These tests verify the login process, including scenarios for valid credentials,
/// invalid credentials, and missing authorization headers.
/// </remarks>
[Trait("Category", "Acceptance")]
public class LoginTests
{
    private readonly LoginStepDefinitions _steps = new();

    /// <summary>
    /// Test case for successful login with valid credentials.
    /// </summary>
    [Fact]
    public void SuccessfulLogin()
    {
        _steps.GivenJwtSettings("test_secret_key_1234567890!@#$%^", "test_issuer", "test_audience")
              .GivenAUserWithCredentials("Marc", "marc")
              .WhenTheUserAttemptsToLogIn()
              .ThenTheLoginIsSuccessful();
    }

    /// <summary>
    /// Test case for attempting to log in with invalid credentials.
    /// </summary>
    [Fact]
    public void LoginWithInvalidCredentials()
    {
        _steps.GivenJwtSettings("test_secret_key_1234567890!@#$%^", "test_issuer", "test_audience")
              .GivenAUserWithCredentials("InvalidUser", "InvalidPassword")
              .WhenTheUserAttemptsToLogIn()
              .ThenAnUnauthorizedErrorIsReturned();
    }

    /// <summary>
    /// Test case for attempting to log in without an authorization header.
    /// </summary>
    [Fact]
    public void MissingAuthorizationHeader()
    {
        _steps.GivenJwtSettings("test_secret_key_1234567890!@#$%^", "test_issuer", "test_audience")
              .GivenAUserWithoutAuthorizationHeader()
              .WhenTheUserAttemptsToLogIn()
              .ThenABadRequestErrorIsReturned();
    }
}

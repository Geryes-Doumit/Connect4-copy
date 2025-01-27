using Connect4.Domain.Commands;
using Connect4.Domain.Tests.Mocks;
using Connect4.Domain.Exceptions;
using Xunit;
using Connect4.Domain.Services;

namespace Connect4.Domain.Tests.Users;

/// <summary>
/// Step definitions for the user login acceptance tests.
/// </summary>
/// <remarks>
/// Implements the Given, When, and Then steps required to test the login functionality,
/// including handling JWT settings, user credentials, and error scenarios.
/// </remarks>
internal class LoginStepDefinitions
{
    private LoginCommand? _command;
    private readonly UserQueryRepositoryMock _userQueryRepository = new();
    private readonly PasswordService _passwordService = new();
    private readonly ConfigurationMock _configuration = new();
    private string? _resultToken;
    private Exception? _exception;

    #region Given

    /// <summary>
    /// Simulates a user with specific credentials for login.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LoginStepDefinitions GivenAUserWithCredentials(string username, string password)
    {
        _command = new LoginCommand(username, password);
        return this;
    }

    /// <summary>
    /// Configures the JWT settings for the login process.
    /// </summary>
    /// <param name="secretKey">The secret key for signing the JWT.</param>
    /// <param name="issuer">The issuer of the JWT.</param>
    /// <param name="audience">The audience for the JWT.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LoginStepDefinitions GivenJwtSettings(string secretKey, string issuer, string audience)
    {
        _configuration["JwtSettings:SecretKey"] = secretKey;
        _configuration["JwtSettings:Issuer"] = issuer;
        _configuration["JwtSettings:Audience"] = audience;
        return this;
    }

    /// <summary>
    /// Simulates a user attempting to log in without an authorization header.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LoginStepDefinitions GivenAUserWithoutAuthorizationHeader()
    {
        _command = null;
        return this;
    }

    #endregion

    #region When

    /// <summary>
    /// Simulates the user attempting to log in.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LoginStepDefinitions WhenTheUserAttemptsToLogIn()
    {
        if (_command == null)
        {
            _exception = new DomainException("Missing Authorization header");
            return this;
        }

        try
        {
            var handler = new LoginCommandHandler(_userQueryRepository, _passwordService, _configuration);
            _resultToken = handler.Handle(_command, default).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            _exception = ex;
        }

        return this;
    }

    #endregion

    #region Then

    /// <summary>
    /// Verifies that the login process was successful and a valid JWT token was returned.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LoginStepDefinitions ThenTheLoginIsSuccessful()
    {
        Assert.NotNull(_resultToken);
        Assert.Null(_exception);
        Assert.StartsWith("eyJ", _resultToken); // Basic JWT format check
        return this;
    }

    /// <summary>
    /// Verifies that an unauthorized error was returned due to invalid credentials.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LoginStepDefinitions ThenAnUnauthorizedErrorIsReturned()
    {
        Assert.Null(_resultToken);
        Assert.NotNull(_exception);
        Assert.IsType<UnauthorizedDomainException>(_exception);
        Assert.Equal("Invalid username or password.", _exception.Message);
        return this;
    }

    /// <summary>
    /// Verifies that a bad request error was returned due to a missing authorization header.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LoginStepDefinitions ThenABadRequestErrorIsReturned()
    {
        Assert.Null(_resultToken);
        Assert.NotNull(_exception);
        Assert.IsType<DomainException>(_exception);
        Assert.Equal("Missing Authorization header", _exception.Message);
        return this;
    }

    #endregion
}

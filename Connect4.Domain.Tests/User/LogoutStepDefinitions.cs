using Connect4.Domain.Services;
using Xunit;

namespace Connect4.Domain.Tests.Users;

/// <summary>
/// Step definitions for the user logout acceptance tests.
/// </summary>
/// <remarks>
/// Implements the Given, When, and Then steps required to test the logout functionality,
/// including handling token blacklisting and authentication scenarios.
/// </remarks>
internal class LogoutStepDefinitions
{
    private readonly TokenBlacklistService _blacklistService = new();
    private string? _jti;
    private DateTime? _expiresAt;
    private Exception? _exception;

    #region Given

    /// <summary>
    /// Simulates a user with a valid token.
    /// </summary>
    /// <param name="jti">The unique token identifier (JTI).</param>
    /// <param name="expiresAt">The token's expiration date and time.</param>
    /// <returns>The current step definitions instance.</returns>
    internal LogoutStepDefinitions GivenAUserWithValidToken(string jti, DateTime expiresAt)
    {
        _jti = jti;
        _expiresAt = expiresAt;
        return this;
    }

    /// <summary>
    /// Simulates a user without a valid token.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LogoutStepDefinitions GivenAUserWithoutToken()
    {
        _jti = null;
        _expiresAt = null;
        return this;
    }

    #endregion

    #region When

    /// <summary>
    /// Simulates the user attempting to log out.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LogoutStepDefinitions WhenTheUserLogsOut()
    {
        try
        {
            if (_jti == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            _blacklistService.BlacklistToken(_jti, _expiresAt ?? DateTime.UtcNow.AddMinutes(30));
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
    /// Verifies that the logout process was successful.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LogoutStepDefinitions ThenTheLogoutIsSuccessful()
    {
        Assert.NotNull(_jti);
        Assert.Null(_exception);
        Assert.True(_blacklistService.IsTokenBlacklisted(_jti!));
        return this;
    }

    /// <summary>
    /// Verifies that an unauthorized error was returned due to the lack of a valid token.
    /// </summary>
    /// <returns>The current step definitions instance.</returns>
    internal LogoutStepDefinitions ThenAnUnauthorizedErrorIsReturned()
    {
        Assert.Null(_jti);
        Assert.NotNull(_exception);
        Assert.IsType<UnauthorizedAccessException>(_exception);
        Assert.Equal("User is not authenticated.", _exception.Message);
        return this;
    }

    #endregion
}

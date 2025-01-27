using System;
using Xunit;
using Connect4.Domain.Services;

namespace Connect4.Domain.Services.UnitTests;

/// <summary>
/// Unit tests for the <see cref="TokenBlacklistService"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class TokenBlacklistServiceTest
{
    private readonly ITokenBlacklistService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenBlacklistServiceTest"/> class.
    /// </summary>
    public TokenBlacklistServiceTest()
    {
        _service = new TokenBlacklistService();
    }

    /// <summary>
    /// Tests that a token can be blacklisted and then checked for blacklisting.
    /// </summary>
    [Fact]
    public void BlacklistToken_AddsTokenToBlacklist()
    {
        // Arrange
        var jti = "test_jti";
        var expiresAt = DateTime.UtcNow.AddMinutes(10);

        // Act
        _service.BlacklistToken(jti, expiresAt);

        // Assert
        Assert.True(_service.IsTokenBlacklisted(jti));
    }

    /// <summary>
    /// Tests that the service correctly identifies blacklisted tokens.
    /// </summary>
    [Fact]
    public void IsTokenBlacklisted_ReturnsTrueForBlacklistedToken()
    {
        // Arrange
        var jti = "test_jti";
        var expiresAt = DateTime.UtcNow.AddMinutes(10);
        _service.BlacklistToken(jti, expiresAt);

        // Act
        var result = _service.IsTokenBlacklisted(jti);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests that the service correctly identifies non-blacklisted tokens.
    /// </summary>
    [Fact]
    public void IsTokenBlacklisted_ReturnsFalseForNonBlacklistedToken()
    {
        // Arrange
        var jti = "non_blacklisted_jti";

        // Act
        var result = _service.IsTokenBlacklisted(jti);

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Tests that the service correctly identifies expired blacklisted tokens.
    /// </summary>
    [Fact]
    public void IsTokenBlacklisted_ReturnsFalseForExpiredBlacklistedToken()
    {
        // Arrange
        var jti = "expired_jti";
        var expiresAt = DateTime.UtcNow.AddMinutes(-10);
        _service.BlacklistToken(jti, expiresAt);

        // Act
        var result = _service.IsTokenBlacklisted(jti);

        // Assert
        Assert.False(result);
    }
}
using System;
using Xunit;
using Connect4.Domain.Services;

namespace Connect4.Domain.Services.UnitTests;

/// <summary>
/// Unit tests for <see cref="PasswordService"/>.
/// </summary>
[Trait("Category", "Unit")]
public class PasswordServiceTest
{
    private readonly IPasswordService _passwordService;

    /// <summary>
    /// Initializes a new instance of <see cref="PasswordServiceTest"/>.
    /// </summary>
    public PasswordServiceTest()
    {
        _passwordService = new PasswordService();
    }

    /// <summary>
    /// Tests that <see cref="IPasswordService.HashPassword(string)"/> returns a non-null and non-empty hash.
    /// </summary>
    [Fact]
    public void HashPassword_ReturnsNonNullOrEmptyHash()
    {
        // Arrange
        var password = "testPassword";

        // Act
        var hash = _passwordService.HashPassword(password);

        // Assert
        Assert.False(string.IsNullOrEmpty(hash));
    }

    /// <summary>
    /// Tests that <see cref="IPasswordService.HashPassword(string)"/> returns a different hash for different passwords.
    /// </summary>
    [Fact]
    public void HashPassword_DifferentPasswords_ReturnDifferentHashes()
    {
        // Arrange
        var password1 = "testPassword1";
        var password2 = "testPassword2";

        // Act
        var hash1 = _passwordService.HashPassword(password1);
        var hash2 = _passwordService.HashPassword(password2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    /// <summary>
    /// Tests that <see cref="IPasswordService.HashPassword(string)"/> returns a different hash for the same password.
    /// </summary>
    [Fact]
    public void HashPassword_SamePassword_ReturnDifferentHashes()
    {
        // Arrange
        var password = "testPassword";

        // Act
        var hash1 = _passwordService.HashPassword(password);
        var hash2 = _passwordService.HashPassword(password);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    /// <summary>
    /// Tests that <see cref="IPasswordService.VerifyPassword(string, string)"/> returns <c>true</c> for a correct password.
    /// </summary>
    [Fact]
    public void VerifyPassword_CorrectPassword_ReturnsTrue()
    {
        // Arrange
        var password = "testPassword";
        var hash = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassword(password, hash);

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Tests that <see cref="IPasswordService.VerifyPassword(string, string)"/> returns <c>false</c> for an incorrect password.
    /// </summary>
    [Fact]
    public void VerifyPassword_IncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var password = "testPassword";
        var incorrectPassword = "wrongPassword";
        var hash = _passwordService.HashPassword(password);

        // Act
        var result = _passwordService.VerifyPassword(incorrectPassword, hash);

        // Assert
        Assert.False(result);
    }
}
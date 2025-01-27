using Xunit;
using Connect4.Domain.Commands;

namespace Connect4.Domain.Commands.UnitTests;

/// <summary>
/// Unit tests for the <see cref="LoginCommand"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class LoginCommandTest
{
    /// <summary>
    /// Tests that the constructor sets the properties correctly.
    /// </summary>
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var username = "TestUser";
        var password = "TestPassword";

        // Act
        var command = new LoginCommand(username, password);

        // Assert
        Assert.Equal(username, command.Username);
        Assert.Equal(password, command.Password);
    }

    /// <summary>
    /// Tests that the <see cref="LoginCommand.Username"/> property returns the correct value.
    /// </summary>
    [Fact]
    public void UsernameProperty_ReturnsCorrectValue()
    {
        // Arrange
        var username = "TestUser";
        var password = "TestPassword";

        // Act
        var command = new LoginCommand(username, password);

        // Assert
        Assert.Equal(username, command.Username);
    }

    /// <summary>
    /// Tests that the <see cref="LoginCommand.Password"/> property returns the correct value.
    /// </summary>
    [Fact]
    public void PasswordProperty_ReturnsCorrectValue()
    {
        // Arrange
        var username = "TestUser";
        var password = "TestPassword";

        // Act
        var command = new LoginCommand(username, password);

        // Assert
        Assert.Equal(password, command.Password);
    }
}
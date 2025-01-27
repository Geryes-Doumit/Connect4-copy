using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Connect4.Domain.Commands;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using Connect4.Domain.Services;
using Microsoft.Extensions.Configuration;
using Connect4.Common.Model;
using System.Collections.Generic;

namespace Connect4.Domain.Commands.UnitTests;

/// <summary>
/// Unit tests for <see cref="LoginCommandHandler"/>.
/// </summary>
[Trait("Category", "Unit")]
public class LoginCommandHandlerTest
{
    private readonly Mock<UserQueryRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly LoginCommandHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandHandlerTest"/> class.
    /// </summary>
    public LoginCommandHandlerTest()
    {
        _userRepositoryMock = new Mock<UserQueryRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _configurationMock = new Mock<IConfiguration>();
        _handler = new LoginCommandHandler(
            _userRepositoryMock.Object,
            _passwordServiceMock.Object,
            _configurationMock.Object
        );
    }

    /// <summary>
    /// Tests that the handler throws an <see cref="UnauthorizedDomainException"/> when the user is not found.
    /// </summary>
    [Fact]
    public async Task Handle_UserNotFound_ThrowsUnauthorizedDomainException()
    {
        // Arrange
        var command = new LoginCommand("TestUser", "TestPassword");
        _userRepositoryMock.Setup(repo => repo.GetUserByUsername("TestUser"))
                           .ReturnsAsync((User)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedDomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Invalid username or password.", exception.Message);
    }

    /// <summary>
    /// Tests that the handler throws an <see cref="UnauthorizedDomainException"/> when the password is invalid.
    /// </summary>
    [Fact]
    public async Task Handle_InvalidPassword_ThrowsUnauthorizedDomainException()
    {
        // Arrange
        var command = new LoginCommand("TestUser", "TestPassword");
        var user = new User(1, "TestUser", "HashedPassword");
        _userRepositoryMock.Setup(repo => repo.GetUserByUsername("TestUser"))
                           .ReturnsAsync(user);
        _passwordServiceMock.Setup(service => service.VerifyPassword("TestPassword", "HashedPassword"))
                            .Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedDomainException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Invalid username or password.", exception.Message);
    }

    /// <summary>
    /// Tests that the handler returns a JWT token when the credentials are valid.
    /// </summary>
    [Fact]
    public async Task Handle_ValidCredentials_ReturnsJwtToken()
    {
        // Arrange
        var command = new LoginCommand("TestUser", "TestPassword");
        var user = new User(1, "TestUser", "HashedPassword");
        _userRepositoryMock.Setup(repo => repo.GetUserByUsername("TestUser"))
                           .ReturnsAsync(user);
        _passwordServiceMock.Setup(service => service.VerifyPassword("TestPassword", "HashedPassword"))
                            .Returns(true);
        _configurationMock.Setup(config => config["JwtSettings:SecretKey"]).Returns("Super_Secret_Key0123456789*!/;117");
        _configurationMock.Setup(config => config["JwtSettings:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(config => config["JwtSettings:Audience"]).Returns("TestAudience");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<string>(result);
    }
}
using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Connect4.API.v1.Controllers;
using Connect4.Domain.Commands;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Services;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using Connect4.Common.Model;

namespace Connect4.API.UnitTests;

/// <summary>
/// Unit tests for the <see cref="UserController"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class UserControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ITokenBlacklistService> _blacklistServiceMock;
    private readonly UserController _controller;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserControllerTest"/> class.
    /// Sets up mocked <see cref="IMediator"/> and <see cref="ITokenBlacklistService"/>, and initializes a <see cref="UserController"/>.
    /// </summary>
    public UserControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _blacklistServiceMock = new Mock<ITokenBlacklistService>();
        _controller = new UserController(_mediatorMock.Object);
    }

    /// <summary>
    /// Tests the Login method to ensure it throws a <see cref="DomainException"/> when the Authorization header is missing.
    /// </summary>
    [Fact]
    public async Task Login_MissingAuthorizationHeader_ReturnsBadRequest()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _controller.Login());
    }

    /// <summary>
    /// Tests the Login method to ensure it throws a <see cref="DomainException"/> for an invalid Authorization header type.
    /// </summary>
    [Fact]
    public async Task Login_InvalidAuthorizationHeaderType_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer token";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _controller.Login());
    }

    /// <summary>
    /// Tests the Login method to ensure it throws a <see cref="DomainException"/> for an invalid Base64 format.
    /// </summary>
    [Fact]
    public async Task Login_InvalidBase64Format_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Basic invalidbase64";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _controller.Login());
    }

    /// <summary>
    /// Tests the Login method to ensure it throws a <see cref="DomainException"/> for invalid credentials format.
    /// </summary>
    [Fact]
    public async Task Login_InvalidCredentialsFormat_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var invalidCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("invalidformat"));
        context.Request.Headers["Authorization"] = $"Basic {invalidCredentials}";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _controller.Login());
    }

    /// <summary>
    /// Tests the Login method to ensure it returns a valid token for correct credentials.
    /// </summary>
    [Fact]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var validCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("Marc:marc"));
        context.Request.Headers["Authorization"] = $"Basic {validCredentials}";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<LoginCommand>(), default))
                    .ReturnsAsync("token");

        // Act
        var result = await _controller.Login();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Asserter sur le type concret
        var response = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.Equal("Login successful", response.Message);
        Assert.Equal("token", response.Token);
    }

    /// <summary>
    /// Tests the Logout method to ensure it throws a <see cref="DomainException"/> if no JTI is found in the token.
    /// </summary>
    [Fact]
    public void Logout_NoJtiInToken_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, "Marc")
};
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => _controller.Logout());
        Assert.Equal("No JTI found in token.", exception.Message);
    }

    /// <summary>
    /// Tests the Logout method to ensure it throws a <see cref="DomainException"/> if no expiration is found in the token.
    /// </summary>
    [Fact]
    public void Logout_NoExpInToken_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var claims = new List<Claim>
{
    new Claim(JwtRegisteredClaimNames.Jti, "jti_value")
};
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => _controller.Logout());
        Assert.Equal("No 'exp' found in token.", exception.Message);
    }

    /// <summary>
    /// Tests the Logout method to ensure it invalidates a valid token and returns an HTTP 200 status code.
    /// </summary>
    [Fact]
    public void Logout_ValidToken_ReturnsOk()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var claims = new List<Claim>
{
    new Claim(JwtRegisteredClaimNames.Jti, "jti_value"),
    new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds().ToString())
};
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
        context.RequestServices = new ServiceCollection()
            .AddSingleton(_blacklistServiceMock.Object)
            .BuildServiceProvider();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act
        var result = _controller.Logout();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<MessageResponsDto>(okResult.Value);
        Assert.Equal("Token has been invalidated (logged out)", response.Message);
        _blacklistServiceMock.Verify(service => service.BlacklistToken("jti_value", It.IsAny<DateTime>()), Times.Once);
    }

}
using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Connect4.API.v1.Controllers;
using Connect4.Domain.Commands;
using Connect4.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Connect4.Domain.Queries;
using Connect4.Domain.Exceptions;

namespace Connect4.API.UnitTests;

/// <summary>
/// Tests for the <see cref="GameController"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class GameControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GameController _controller;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameControllerTests"/> class.
    /// Sets up a mocked <see cref="IMediator"/> and an instance of <see cref="GameController"/>.
    /// </summary>
    public GameControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new GameController(_mediatorMock.Object);
    }

    /// <summary>
    /// Tests the CreateGame method to ensure it returns an OkResult with the correct GameId.
    /// </summary>
    [Fact]
    public async Task CreateGame_ReturnsOkResult_WithGameId()
    {
        // Arrange
        var userName = "testuser";
        var gameName = "Test Game";
        var gameId = 1;

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateGameCommand>(), default))
                     .ReturnsAsync(gameId);

        var context = new DefaultHttpContext();
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act
        var result = await _controller.CreateGame(gameName);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<GameIdResponsDto>(okResult.Value);
        Assert.Equal("Game created successfully.", returnValue.Message);
        Assert.Equal(gameId, returnValue.GameId);
    }

    /// <summary>
    /// Tests the JoinGame method to ensure it returns an OkResult with the correct GameId.
    /// </summary>
    [Fact]
    public async Task JoinGame_ReturnsOkResult_WithGameId()
    {
        // Arrange
        var userName = "testuser";
        var gameId = 1;

        _mediatorMock.Setup(m => m.Send(It.IsAny<JoinGameCommand>(), default))
                     .ReturnsAsync(gameId);

        var context = new DefaultHttpContext();
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act
        var result = await _controller.JoinGame(gameId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<GameIdResponsDto>(okResult.Value);
        Assert.Equal("Game joined successfully.", returnValue.Message);
        Assert.Equal(gameId, returnValue.GameId);
    }

    /// <summary>
    /// Tests the LeaveGame method to ensure it returns an OkResult with the correct message.
    /// </summary>
    [Fact]
    public async Task LeaveGame_ReturnsOkResult_WithMessage()
    {
        // Arrange
        var userName = "testuser";
        var gameId = 1;

        _mediatorMock.Setup(m => m.Send(It.IsAny<LeaveGameCommand>(), default))
                     .ReturnsAsync(Unit.Value);

        var context = new DefaultHttpContext();
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act
        var result = await _controller.LeaveGame(gameId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<MessageResponsDto>(okResult.Value);
        Assert.Equal("Game left successfully.", returnValue.Message);
    }

    /// <summary>
    /// Tests the PlayMove method to ensure it returns an OkResult with the correct message.
    /// </summary>
    [Fact]
    public async Task PlayMove_ReturnsOkResult_WithMessage()
    {
        // Arrange
        var userName = "testuser";
        var gameId = 1;
        var column = 3;

        _mediatorMock.Setup(m => m.Send(It.IsAny<PlayMoveCommand>(), default))
                     .ReturnsAsync(gameId);

        var context = new DefaultHttpContext();
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act
        var result = await _controller.PlayMove(gameId, column);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<MessageResponsDto>(okResult.Value);
        Assert.Equal("Move played successfully.", returnValue.Message);
    }

    /// <summary>
    /// Tests the GetGame method to ensure it returns an OkResult with the correct GameDetail.
    /// </summary>
    [Fact]
    public async Task GetGame_ReturnsOkResult_WithPlayingGameDto()
    {
        // Arrange
        var userName = "testuser";
        var gameId = 1;
        var playingGameDto = new GameDetail(new Connect4Game(gameId, "Test Game 1", userName, "InProgress", new("testuser", "0000000;0000000;0000000;0000000;0000000;0000000"), "testuser3"));

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetPlayingGameQuery>(), default))
                     .ReturnsAsync(playingGameDto);

        var context = new DefaultHttpContext();
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);
        context.User = principal;
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act
        var result = await _controller.GetGame(gameId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<GameDetail>(okResult.Value);
        Assert.Equal(gameId, returnValue.GameId);
        Assert.Equal(userName, returnValue.Host);
    }

    /// <summary>
    /// Tests the CreateGame method to ensure it throws UnauthorizedDomainException for unauthorized users.
    /// </summary>
    [Fact]
    public async Task CreateGame_UnauthorizedUser_ThrowsUnauthorizedDomainException()
    {
        // Arrange
        var gameName = "Test Game";

        var context = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedDomainException>(() => _controller.CreateGame(gameName));
    }

    /// <summary>
    /// Tests the GetGame method to ensure it throws UnauthorizedDomainException for unauthorized users.
    /// </summary>
    [Fact]
    public async Task GetGame_UnauthorizedUser_ThrowsUnauthorizedDomainException()
    {
        // Arrange
        var gameId = 1;

        var context = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedDomainException>(() => _controller.GetGame(gameId));
    }

    /// <summary>
    /// Tests the JoinGame method to ensure it throws UnauthorizedDomainException for unauthorized users.
    /// </summary>
    [Fact]
    public async Task JoinGame_UnauthorizedUser_ThrowsUnauthorizedDomainException()
    {
        // Arrange
        var gameId = 1;

        var context = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedDomainException>(() => _controller.JoinGame(gameId));
    }

    /// <summary>
    /// Tests the LeaveGame method to ensure it throws UnauthorizedDomainException for unauthorized users.
    /// </summary>
    [Fact]
    public async Task LeaveGame_UnauthorizedUser_ThrowsUnauthorizedDomainException()
    {
        // Arrange
        var gameId = 1;

        var context = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedDomainException>(() => _controller.LeaveGame(gameId));
    }

    /// <summary>
    /// Tests the PlayMove method to ensure it throws UnauthorizedDomainException for unauthorized users.
    /// </summary>
    [Fact]
    public async Task PlayMove_UnauthorizedUser_ThrowsUnauthorizedDomainException()
    {
        // Arrange
        var gameId = 1;
        var column = 3;

        var context = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedDomainException>(() => _controller.PlayMove(gameId, column));
    }
}

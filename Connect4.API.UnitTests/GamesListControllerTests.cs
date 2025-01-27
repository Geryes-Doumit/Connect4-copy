using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Connect4.API.v1.Controllers;
using Connect4.Domain.Queries;
using Connect4.Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Connect4.Domain.Exceptions;

namespace Connect4.API.UnitTests;

/// <summary>
/// Unit tests for the <see cref="GamesListController"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class GamesListControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly GamesListController _controller;

    /// <summary>
    /// Initializes a new instance of the <see cref="GamesListControllerTests"/> class.
    /// Sets up a mocked <see cref="IMediator"/> and an instance of <see cref="GamesListController"/>.
    /// </summary>
    public GamesListControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new GamesListController(_mediatorMock.Object);
    }

    /// <summary>
    /// Tests the GetWaitingGames method to ensure it returns a list of waiting games with an HTTP 200 status code.
    /// </summary>
    [Fact]
    public async Task GetWaitingGames_ReturnsOkResult_WithListOfWaitingGames()
    {
        // Arrange
        var userName = "testuser";
        var waitingGames = new List<WaitingGameDto>
        {
            new WaitingGameDto(new Connect4Game(1, "Test Game 1", "testuser1", "Waiting", new("testuser1", "0000000;0000000;0000000;0000000;0000000;0000000"))),
            new WaitingGameDto(new Connect4Game(1, "Test Game 2", "testuser2", "Waiting", new("testuser2", "0000000;0000000;0000000;0000000;0000000;0000000"))),
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetWaitingGamesQuery>(), default))
                     .ReturnsAsync(waitingGames);

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
        var result = await _controller.GetWaitingGames(null, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<WaitingGameDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    /// <summary>
    /// Tests the GetFinishedGames method to ensure it returns a list of finished games with an HTTP 200 status code.
    /// </summary>
    [Fact]
    public async Task GetFinishedGames_ReturnsOkResult_WithListOfFinishedGames()
    {
        // Arrange
        var userName = "testuser";
        var finishedGames = new List<FinishedGameDto>
        {
            new FinishedGameDto(new Connect4Game(1, "Test Game 1", "testuser1", "Finished", new("testuser1", "0000000;0000000;0000000;0000000;0000000;0000000"), "testuser3", "testuser1")),
            new FinishedGameDto(new Connect4Game(1, "Test Game 2", "testuser2", "Finished", new("testuser2", "0000000;0000000;0000000;0000000;0000000;0000000"), "testuser4", "testuser2")),
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetFinishedGamesQuery>(), default))
                     .ReturnsAsync(finishedGames);

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
        var result = await _controller.GetFinishedGames(null, null);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<List<FinishedGameDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    /// <summary>
    /// Tests the GetWaitingGames method to ensure it throws an <see cref="UnauthorizedDomainException"/> when the user is not authenticated.
    /// </summary>
    [Fact]
    public async Task GetWaitingGames_Unauthorized_ReturnsUnauthorizedResult()
    {
        // Arrange
        var context = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedDomainException>(() => _controller.GetWaitingGames(null, null));

    }

    /// <summary>
    /// Tests the GetFinishedGames method to ensure it throws an <see cref="UnauthorizedDomainException"/> when the user is not authenticated.
    /// </summary>
    [Fact]
    public async Task GetFinishedGames_Unauthorized_ReturnsUnauthorizedResult()
    {
        // Arrange
        var context = new DefaultHttpContext();
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedDomainException>(() => _controller.GetFinishedGames(null, null));
    }
}

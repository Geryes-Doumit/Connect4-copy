using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using Xunit;
using Connect4.API;
using Connect4.Domain.Exceptions;
using Connect4.Common.Model;
using Microsoft.AspNetCore.Http;

namespace Connect4.API.UnitTests;

/// <summary>
/// Unit tests for the <see cref="HttpResponseExceptionFilter"/> class.
/// </summary>
[Trait("Category", "Unit")]
public class HttpResponseExceptionFilterTests
{
    private readonly HttpResponseExceptionFilter _filter;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpResponseExceptionFilterTests"/> class.
    /// Sets up an instance of <see cref="HttpResponseExceptionFilter"/>.
    /// </summary>
    public HttpResponseExceptionFilterTests()
    {
        _filter = new HttpResponseExceptionFilter();
    }

    /// <summary>
    /// Tests that the filter sets the HTTP status code to 401 for an <see cref="UnauthorizedDomainException"/>.
    /// </summary>
    [Fact]
    public void OnException_UnauthorizedDomainException_Returns401()
    {
        // Arrange
        var exception = new UnauthorizedDomainException("Unauthorized access");
        var httpContext = new DefaultHttpContext();
        var context = new ExceptionContext(
            new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>())
        {
            Exception = exception
        };

        // Act
        _filter.OnException(context);

        // Assert
        var result = Assert.IsType<JsonResult>(context.Result);
        Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        var response = Assert.IsType<MessageResponsDto>(result.Value);
        Assert.Equal("Unauthorized access", response.Message);
    }

    /// <summary>
    /// Tests that the filter sets the HTTP status code to 409 for a <see cref="BusyUserDomainException"/>.
    /// </summary>
    [Fact]
    public void OnException_BusyUserDomainException_Returns409()
    {
        // Arrange
        var exception = new BusyUserDomainException("User is busy", 1);
        var httpContext = new DefaultHttpContext();
        var context = new ExceptionContext(
            new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>())
        {
            Exception = exception
        };

        // Act
        _filter.OnException(context);

        // Assert
        var result = Assert.IsType<JsonResult>(context.Result);
        Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
        var response = Assert.IsType<GameIdResponsDto>(result.Value);
        Assert.Equal("User is busy", response.Message);
        Assert.Equal(1, response.GameId);
    }

    /// <summary>
    /// Tests that the filter sets the HTTP status code to 404 for a <see cref="GameNotFoundException"/>.
    /// </summary>
    [Fact]
    public void OnException_GameNotFoundException_Returns404()
    {
        // Arrange
        var exception = new GameNotFoundException("Game not found");
        var httpContext = new DefaultHttpContext();
        var context = new ExceptionContext(
            new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>())
        {
            Exception = exception
        };

        // Act
        _filter.OnException(context);

        // Assert
        var result = Assert.IsType<JsonResult>(context.Result);
        Assert.Equal(StatusCodes.Status404NotFound, result.StatusCode);
        var response = Assert.IsType<MessageResponsDto>(result.Value);
        Assert.Equal("Game not found", response.Message);
    }

    /// <summary>
    /// Tests that the filter sets the HTTP status code to 403 for a <see cref="NotYourTurnDomainException"/>.
    /// </summary>
    [Fact]
    public void OnException_NotYourTurnDomainException_Returns403()
    {
        // Arrange
        var exception = new NotYourTurnDomainException("Not your turn");
        var httpContext = new DefaultHttpContext();
        var context = new ExceptionContext(
            new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>())
        {
            Exception = exception
        };

        // Act
        _filter.OnException(context);

        // Assert
        var result = Assert.IsType<JsonResult>(context.Result);
        Assert.Equal(StatusCodes.Status403Forbidden, result.StatusCode);
        var response = Assert.IsType<MessageResponsDto>(result.Value);
        Assert.Equal("Not your turn", response.Message);
    }

    /// <summary>
    /// Tests that the filter sets the HTTP status code to 400 for a general <see cref="DomainException"/>.
    /// </summary>
    [Fact]
    public void OnException_DomainException_Returns400()
    {
        // Arrange
        var exception = new DomainException("Bad request");
        var httpContext = new DefaultHttpContext();
        var context = new ExceptionContext(
            new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>())
        {
            Exception = exception
        };

        // Act
        _filter.OnException(context);

        // Assert
        var result = Assert.IsType<JsonResult>(context.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        var response = Assert.IsType<MessageResponsDto>(result.Value);
        Assert.Equal("Bad request", response.Message);
    }

    /// <summary>
    /// Tests that the filter sets the HTTP status code to 500 for any other <see cref="Exception"/>.
    /// </summary>
    [Fact]
    public void OnException_OtherException_Returns500()
    {
        // Arrange
        var exception = new Exception("Internal server error");
        var httpContext = new DefaultHttpContext();
        var context = new ExceptionContext(
            new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()),
            new List<IFilterMetadata>())
        {
            Exception = exception
        };

        // Act
        _filter.OnException(context);

        // Assert
        var result = Assert.IsType<JsonResult>(context.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        var response = Assert.IsType<MessageResponsDto>(result.Value);
        Assert.Equal("An error occurred", response.Message);
    }
}


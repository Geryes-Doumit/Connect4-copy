using Connect4.Common.Model;
using Connect4.Domain.Commands;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Connect4.API.v1.Controllers;

/// <summary>
/// Controller for managing game-related actions.
/// </summary>
/// <remarks>
/// Provides endpoints to create, join, leave, and play moves in games.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class GameController : Controller
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling commands.</param>
    public GameController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This endpoint allows an authenticated user to create a new game by providing a valid game name.
    /// </para>
    /// <para>Possible HTTP status codes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><strong>200 OK</strong> - Game created successfully.</description>
    /// </item>
    /// <item>
    /// <description><strong>401 Unauthorized</strong> - The user is not authenticated.</description>
    /// </item>
    /// <item>
    /// <description><strong>400 Bad Request</strong> - Invalid game name (e.g., empty, too short, too long).</description>
    /// </item>
    /// <item>
    /// <description><strong>409 Conflict</strong> - Conflict in creating the game (e.g., user is already in a game).</description>
    /// </item>
    /// <item>
    /// <description><strong>500 Internal Server Error</strong> - Unexpected internal error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="gameName">The name of the game to create.</param>
    /// <returns>
    /// Returns a JSON object containing the <see cref="GameIdResponsDto"/> if the game is created successfully.
    /// In case of an error, a JSON object containing the <see cref="MessageResponsDto"/> is returned.
    /// </returns>
    [HttpPost("")]
    [Authorize]
    [ProducesResponseType(typeof(GameIdResponsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateGame([FromBody] string gameName)
    {
        // Retrieve the username from the token
        var userNameClaim = User.FindFirst(ClaimTypes.Name);
        if (userNameClaim == null)
        {
            throw new UnauthorizedDomainException("User is not authenticated.");
        }
        var userName = userNameClaim.Value;

        var command = new CreateGameCommand(gameName, userName);
        var result = await _mediator.Send(command);

        return Ok(new GameIdResponsDto
        {
            Message = "Game created successfully.",
            GameId = result
        });
    }


    /// <summary>
    /// Gets a game by its ID.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This endpoint allows an authenticated user to retrieve a game by its ID. Used for refreshing the game state.
    /// </para>
    /// <para>Possible HTTP status codes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><strong>200 OK</strong> - Game get successfully.</description>
    /// </item>
    /// <item>
    /// <description><strong>401 Unauthorized</strong> - The user is not authenticated.</description>
    /// </item>
    /// <item>
    /// <description><strong>400 Bad Request</strong> - Invalid game name (e.g., empty, too short, too long).</description>
    /// </item>
    /// <item>
    /// <description><strong>409 Conflict</strong> - Conflict in creating the game (e.g., user is already in a game).</description>
    /// </item>
    /// <item>
    /// <description><strong>500 Internal Server Error</strong> - Unexpected internal error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="gameId">The ID of the game to retrieve.</param>
    /// <returns>
    /// Returns a JSON object containing the <see cref="GameDetail"/> if the game is retrieved successfully.
    /// In case of an error, a JSON object containing the <see cref="MessageResponsDto"/> is returned.
    /// </returns>
    [HttpGet("{gameId}")]
    [Authorize]
    [ProducesResponseType(typeof(GameDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetGame([FromRoute] int gameId)
    {
        var userNameClaim = User.FindFirst(ClaimTypes.Name);
        if (userNameClaim == null)
        {
            throw new UnauthorizedDomainException("User is not authenticated.");
        }
        var userName = userNameClaim.Value;

        var query = new GetPlayingGameQuery(gameId, userName);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Joins an existing game.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This endpoint allows an authenticated user to join an existing game by providing the game's ID.
    /// </para>
    /// <para>Possible HTTP status codes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><strong>200 OK</strong> - Game joined successfully.</description>
    /// </item>
    /// <item>
    /// <description><strong>401 Unauthorized</strong> - The user is not authenticated.</description>
    /// </item>
    /// <item>
    /// <description><strong>400 Bad Request</strong> - Invalid game ID or request format.</description>
    /// </item>
    /// <item>
    /// <description><strong>404 Not Found</strong> - The specified game does not exist.</description>
    /// </item>
    /// <item>
    /// <description><strong>409 Conflict</strong> - Conflict in joining the game (e.g., game is full).</description>
    /// </item>
    /// <item>
    /// <description><strong>500 Internal Server Error</strong> - Unexpected internal error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="gameId">The ID of the game to join.</param>
    /// <returns>
    /// Returns a JSON object containing the <see cref="GameIdResponsDto"/> if the game is joined successfully.
    /// In case of an error, a JSON object containing the <see cref="MessageResponsDto"/> is returned.
    /// </returns>
    [HttpPost("{gameId}/join")]
    [Authorize]
    [ProducesResponseType(typeof(GameIdResponsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> JoinGame([FromRoute] int gameId)
    {
        // Retrieve the username from the token
        var userNameClaim = User.FindFirst(ClaimTypes.Name);
        if (userNameClaim == null)
        {
            throw new UnauthorizedDomainException("User is not authenticated.");
        }
        var userName = userNameClaim.Value;
        var command = new JoinGameCommand(gameId, userName);
        await _mediator.Send(command);
        return Ok(new GameIdResponsDto
        {
            Message = "Game joined successfully.",
            GameId = gameId
        });
    }

    /// <summary>
    /// Leaves an existing game.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This endpoint allows an authenticated user to leave a game by providing the game's ID.
    /// </para>
    /// <para>Possible HTTP status codes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><strong>200 OK</strong> - Game left successfully.</description>
    /// </item>
    /// <item>
    /// <description><strong>401 Unauthorized</strong> - The user is not authenticated.</description>
    /// </item>
    /// <item>
    /// <description><strong>400 Bad Request</strong> - Invalid game ID or request format.</description>
    /// </item>
    /// <item>
    /// <description><strong>404 Not Found</strong> - The specified game does not exist.</description>
    /// </item>
    /// <item>
    /// <description><strong>500 Internal Server Error</strong> - Unexpected internal error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="gameId">The ID of the game to leave.</param>
    /// <returns>
    /// Returns a JSON object containing the <see cref="MessageResponsDto"/> if the game is left successfully.
    /// In case of an error, a JSON object containing the <see cref="MessageResponsDto"/> is returned.
    /// </returns>
    [HttpPost("{gameId}/leave")]
    [Authorize]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LeaveGame([FromRoute] int gameId)
    {
        // Retrieve the username from the token
        var userNameClaim = User.FindFirst(ClaimTypes.Name);
        if (userNameClaim == null)
        {
            throw new UnauthorizedDomainException("User is not authenticated.");
        }
        var userName = userNameClaim.Value;
        var command = new LeaveGameCommand(gameId, userName);
        await _mediator.Send(command);
        //return Ok(new
        //{
        //    message = "Game left successfully."
        //});
        return Ok(new MessageResponsDto
        {
            Message = "Game left successfully."
        });
    }

    /// <summary>
    /// Plays a move in an existing game.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This endpoint allows an authenticated user to play a move by specifying the game ID and the column number.
    /// </para>
    /// <para>Possible HTTP status codes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><strong>200 OK</strong> - Move played successfully.</description>
    /// </item>
    /// <item>
    /// <description><strong>401 Unauthorized</strong> - The user is not authenticated.</description>
    /// </item>
    /// <item>
    /// <description><strong>400 Bad Request</strong> - Invalid game ID or column number.</description>
    /// </item>
    /// <item>
    /// <description><strong>404 Not Found</strong> - The specified game does not exist.</description>
    /// </item>
    /// <item>
    /// <description><strong>409 Conflict</strong> - Conflict in playing the move (e.g., invalid move).</description>
    /// </item>
    /// <item>
    /// <description><strong>500 Internal Server Error</strong> - Unexpected internal error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="gameId">The ID of the game where the move is to be played.</param>
    /// <param name="column">The column number where the player wants to place their piece.</param>
    /// <returns>
    /// Returns a JSON object containing the <see cref="MessageResponsDto"/> if the move is played successfully.
    /// In case of an error, a JSON object containing the <see cref="MessageResponsDto"/> is returned.
    /// </returns>
    [HttpPost("{gameId}/playMove")]
    [Authorize]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PlayMove([FromRoute] int gameId, [FromBody] int column)
    {
        // Retrieve the username from the token
        var userNameClaim = User.FindFirst(ClaimTypes.Name);
        if (userNameClaim == null)
        {
            throw new UnauthorizedDomainException("User is not authenticated.");
        }
        var userName = userNameClaim.Value;
        var command = new PlayMoveCommand(gameId, userName, column);
        await _mediator.Send(command);

        return Ok(new MessageResponsDto { Message = "Move played successfully." });
    }
}

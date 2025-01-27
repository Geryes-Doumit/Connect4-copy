using MediatR;
using Connect4.Common.Model;
using Connect4.Domain.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Connect4.Domain.Exceptions;

namespace Connect4.API.v1.Controllers;

/// <summary>
/// Controller that handles the list of games (waiting and finished).
/// </summary>
/// <remarks>
/// Provides endpoints to list all games waiting for a player and to retrieve the history of games 
/// where the requester was either the host or the guest.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class GamesListController : Controller
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="GamesListController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR instance for handling commands and queries.</param>
    public GamesListController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lists all games whose status is "WaitingForPlayers".
    /// </summary>
    /// <remarks>
    /// <para>
    /// This endpoint retrieves all games that are currently waiting for players to join.
    /// It supports pagination through optional <c>limit</c> and <c>offset</c> query parameters.
    /// </para>
    /// <para>Possible HTTP status codes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><strong>200 OK</strong> - Successfully retrieved the list of waiting games.</description>
    /// </item>
    /// <item>
    /// <description><strong>401 Unauthorized</strong> - The user is not authenticated.</description>
    /// </item>
    /// <item>
    /// <description><strong>400 Bad Request</strong> - Invalid query parameters.</description>
    /// </item>
    /// <item>
    /// <description><strong>409 Conflict</strong> - Conflict in retrieving the games (e.g., user is currently in a game).</description>
    /// </item>
    /// <item>
    /// <description><strong>500 Internal Server Error</strong> - Unexpected internal error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="limit">Optional. The maximum number of games to return. Defaults to 10 if not specified.</param>
    /// <param name="offset">Optional. The number of games to skip before starting to collect the result set. Defaults to 0 if not specified.</param>
    /// <returns>
    /// Returns a JSON array of <see cref="WaitingGameDto"</see> objects representing the waiting games.
    /// In case of an error, a <see cref="MessageResponsDto"/> object is returned.
    /// </returns>
    [HttpGet("Waiting")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<WaitingGameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status500InternalServerError)]
    public async Task<OkObjectResult> GetWaitingGames(
        [FromQuery] int? limit,
        [FromQuery] int? offset)
    {
        // Retrieve the username from the token
        var userNameClaim = User.FindFirst(ClaimTypes.Name);
        if (userNameClaim == null)
        {
            throw new UnauthorizedDomainException("User is not authenticated.");
        }
        var userName = userNameClaim.Value;

        // Force categoryFilter to "waiting"
        var query = new GetWaitingGamesQuery("waiting", limit, offset)
        {
            UserName = userName
        };

        var games = await _mediator.Send(query);
        return Ok(games);
    }

    /// <summary>
    /// Lists all games with status "Finished" where the current user is host or guest.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This endpoint retrieves all finished games in which the authenticated user participated 
    /// either as the host or as the guest. It supports pagination through optional <c>limit</c> and <c>offset</c> query parameters.
    /// </para>
    /// <para>Possible HTTP status codes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><strong>200 OK</strong> - Successfully retrieved the list of finished games.</description>
    /// </item>
    /// <item>
    /// <description><strong>401 Unauthorized</strong> - The user is not authenticated.</description>
    /// </item>
    /// <item>
    /// <description><strong>400 Bad Request</strong> - Invalid query parameters.</description>
    /// </item>
    /// <item>
    /// <description><strong>409 Conflict</strong> - Conflict in retrieving the games (e.g., user is currently in a game).</description>
    /// </item>
    /// <item>
    /// <description><strong>500 Internal Server Error</strong> - Unexpected internal error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="limit">Optional. The maximum number of games to return. Defaults to 10 if not specified.</param>
    /// <param name="offset">Optional. The number of games to skip before starting to collect the result set. Defaults to 0 if not specified.</param>
    /// <returns>
    /// Returns a JSON array of <see cref="FinishedGameDto"</see> objects representing the finished games."
    /// In case of an error, a <see cref="MessageResponsDto"/> object is returned.
    /// </returns>
    [HttpGet("History")]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<FinishedGameDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status500InternalServerError)]
    public async Task<OkObjectResult> GetFinishedGames(
        [FromQuery] int? limit,
        [FromQuery] int? offset)
    {
        // Retrieve the username from the token
        var userNameClaim = User.FindFirst(ClaimTypes.Name);
        if (userNameClaim == null)
        {
            throw new UnauthorizedDomainException("User is not authenticated.");
        }
        var userName = userNameClaim.Value;

        // Construct the filter string => "finished-bob" for example
        var filter = $"finished-{userName}";

        var query = new GetFinishedGamesQuery(filter, limit, offset)
        {
            UserName = userName
        };
        var games = await _mediator.Send(query);
        return Ok(games);
    }
}

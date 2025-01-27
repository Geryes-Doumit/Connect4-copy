using MediatR;
using Microsoft.AspNetCore.Mvc;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Commands;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Connect4.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using Connect4.Common.Model;

namespace Connect4.API.v1.Controllers;

/// <summary>
/// Controller that handles user authentication and logout operations.
/// </summary>
/// <remarks>
/// Provides endpoints for Basic => JWT login and token invalidation (logout).
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserController"/> class.
    /// </summary>
    /// <param name="mediator">The MediatR instance for command/query handling.</param>
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Endpoint to authenticate a user (login).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This endpoint expects an "Authorization" header using "Basic" encoding in Base64 format.
    /// The decoded format should be "username:password".
    /// </para>
    /// <para>Possible HTTP status codes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><strong>200 OK</strong> - Valid credentials. Returns a success message and a JWT token.</description>
    /// </item>
    /// <item>
    /// <description><strong>401 Unauthorized</strong> - Invalid username/password.</description>
    /// </item>
    /// <item>
    /// <description><strong>400 Bad Request</strong> - Invalid input format (e.g., missing header, invalid base64, etc.).</description>
    /// </item>
    /// <item>
    /// <description><strong>500 Internal Server Error</strong> - Unexpected internal error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <returns>
    /// Returns a JSON object containing the <see cref="LoginResponseDto"/> if the login is successful.
    /// In case of an error, a JSON object containing the <see cref="MessageResponsDto"/> is returned.
    /// </returns>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login()
    {
        // 1) Check if the "Authorization" header is provided
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            // Missing => 400
            throw new DomainException("Missing Authorization header");
        }

        // 2) Verify that it's "Basic" authentication
        if (!authHeader.Any(x => x.StartsWith("Basic", StringComparison.OrdinalIgnoreCase)))
        {
            // Incorrect format => 400
            throw new DomainException("Invalid Authorization header type");
        }

        // 3) Extract the Base64-encoded credentials after "Basic "
        var encodedCreds = authHeader.ToString()["Basic ".Length..].Trim();
        byte[] credentialBytes;

        // 4) Attempt to decode the Base64 string
        try
        {
            credentialBytes = Convert.FromBase64String(encodedCreds);
        }
        catch
        {
            // Invalid base64 => 400
            throw new DomainException("Invalid Base64 format in 'Authorization' header.");
        }

        // 5) Split into "username:password"
        var credentials = Encoding.UTF8.GetString(credentialBytes);
        var parts = credentials.Split(':', 2);
        if (parts.Length != 2)
        {
            // Incorrect format => 400
            throw new DomainException("Invalid Basic credentials format. Expected 'username:password'.");
        }

        var username = parts[0];
        var password = parts[1];

        // 6) Send command to the handler; if credentials are invalid => UnauthorizedDomainException (401)
        var token = await _mediator.Send(new LoginCommand(username, password));

        // 7) On success => 200 OK
        return Ok(new LoginResponseDto
        {
            Message = "Login successful",
            Token = token
        });
    }

    /// <summary>
    /// Endpoint to log out (invalidate the current JWT token).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This endpoint requires <c>[Authorize]</c>, so a valid JWT must be supplied
    /// in the <c>Authorization: Bearer &lt;token&gt;</c> header.
    /// </para>
    /// <para>
    /// If the token is valid, it retrieves the token's unique identifier (<c>jti</c>) 
    /// and adds it to a blacklist. Any future requests using this token will be rejected (401).
    /// </para>
    /// <para>Possible HTTP status codes:</para>
    /// <list type="bullet">
    /// <item>
    /// <description><strong>200 OK</strong> - Token successfully invalidated.</description>
    /// </item>
    /// <item>
    /// <description><strong>401 Unauthorized</strong> - The token is invalid or not provided.</description>
    /// </item>
    /// <item>
    /// <description><strong>400 Bad Request</strong> - The token is missing required claims (JTI, Exp).</description>
    /// </item>
    /// <item>
    /// <description><strong>500 Internal Server Error</strong> - Unexpected internal error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <returns>
    /// Returns a JSON object containing the <see cref="MessageResponsDto"/> with a message indicating the token has been invalidated.
    /// In case of an error, a JSON object containing the <see cref="MessageResponsDto"/> is returned.
    /// </returns>
    [Authorize]
    [HttpPost("Logout")]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(MessageResponsDto), StatusCodes.Status500InternalServerError)]
    public IActionResult Logout()
    {
        // 1) Retrieve the "jti" claim from the token
        var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (string.IsNullOrEmpty(jti))
        {
            throw new DomainException("No JTI found in token.");
        }

        // 2) Retrieve the "exp" claim (token expiration)
        var expClaim = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
        if (string.IsNullOrEmpty(expClaim))
        {
            throw new DomainException("No 'exp' found in token.");
        }

        // 3) Convert the Unix timestamp "exp" into a DateTime
        var expUnix = long.Parse(expClaim);
        var expiresAt = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

        // 4) Add the token to the blacklist
        var blacklistService = HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();
        blacklistService.BlacklistToken(jti, expiresAt);

        // 5) Return 200 OK
        return Ok(new MessageResponsDto 
        { 
            Message = "Token has been invalidated (logged out)" 
        });
    }
}

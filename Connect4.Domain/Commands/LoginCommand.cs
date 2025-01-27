using MediatR;

namespace Connect4.Domain.Commands
{
    /// <summary>
    /// Represents a command to authenticate a user based on a username and password.
    /// </summary>
    /// <remarks>
    /// This command returns a JWT token if authentication succeeds.
    /// </remarks>
    /// <param name="Username">The username provided by the client.</param>
    /// <param name="Password">The password provided by the client.</param>
    /// <returns>A string representing the issued JWT token upon success.</returns>
    public record LoginCommand(string Username, string Password) : IRequest<string>;
}

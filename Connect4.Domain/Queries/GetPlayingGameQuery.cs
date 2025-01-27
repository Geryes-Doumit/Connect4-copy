using Connect4.Common.Model;
using MediatR;

namespace Connect4.Domain.Queries;

/// <summary>
/// Query to get a specific ongoing game for a user.
/// </summary>
/// <remarks>
/// This query retrieves a specific ongoing game in which the specified user is participating, either as the host or as the guest.
/// </remarks>
/// <param name="gameId">The unique identifier of the game to retrieve.</param>
/// <param name="userName"> The username of the user whose game is being queried.</param>
public record GetPlayingGameQuery(
    int gameId,
    string userName
    ) : IRequest<GameDetail>
{
    /// <summary>
    /// Gets the unique identifier of the game to retrieve.
    /// </summary>
    public int GameId => gameId;

    /// <summary>
    /// Gets the username of the user whose game is being queried.
    /// </summary>
    public string UserName => userName;
}

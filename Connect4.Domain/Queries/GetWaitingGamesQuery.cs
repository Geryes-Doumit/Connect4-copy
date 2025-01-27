using MediatR;
using Connect4.Common.Model;

namespace Connect4.Domain.Queries;

/// <summary>
/// Query to get a list of waiting games.
/// </summary>
/// <remarks>
/// This query retrieves all games that are currently waiting for players to join.
/// Supports pagination through optional <c>Limit</c> and <c>Offset</c> parameters.
/// </remarks>
/// <param name="CategoryFilter">The category filter to apply (e.g., "waiting").</param>
/// <param name="Limit">Optional. The maximum number of games to return. Defaults to 10 if not specified.</param>
/// <param name="Offset">Optional. The number of games to skip before starting to collect the result set. Defaults to 0 if not specified.</param>
public record GetWaitingGamesQuery(
    string CategoryFilter,
    int? Limit = null,
    int? Offset = null
) : IRequest<IEnumerable<WaitingGameDto>>
{
    /// <summary>
    /// Gets the limit for the number of games to retrieve, defaulting to 10 if not specified.
    /// </summary>
    public int LimitOrDefault => Limit ?? 10;

    /// <summary>
    /// Gets the offset for pagination, defaulting to 0 if not specified.
    /// </summary>
    public int OffsetOrDefault => Offset ?? 0;

    /// <summary>
    /// Gets or sets the username of the user whose waiting games are being queried.
    /// </summary>
    public string UserName { get; init; }
}

using Connect4.Common.Model;
using Connect4.Domain.Exceptions;
using Connect4.Domain.Repositories;
using MediatR;

namespace Connect4.Domain.Queries;

/// <summary>
/// Handler for <see cref="GetPlayingGameQuery"/>.
/// Use <see cref="GameQueryRepository"/> to fetch data.
/// </summary>
/// <remarks>
/// This handler retrieves a specific ongoing game in which the specified user is participating, either as the host or as the guest.
/// </remarks>
public class GetPlayingGameQueryHandler : IRequestHandler<GetPlayingGameQuery, GameDetail>
{
    public readonly GameQueryRepository _gameRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetPlayingGameQueryHandler"/> class.
    /// </summary>
    /// <param name="gameRepository"></param>
    public GetPlayingGameQueryHandler(GameQueryRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    /// <summary>
    /// Handles the retrieval of a specific ongoing game by processing the <see cref="GetPlayingGameQuery"/>.
    /// </summary>
    /// <param name="request">The <see cref="GetPlayingGameQuery"/> containing query data.</param>"
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="GameDetail"/> representing the ongoing game.</returns>
    /// <exception cref="GameNotFoundException">
    /// Thrown when the game is not found or the user is not a participant.
    /// </exception>
    public async Task<GameDetail> Handle(GetPlayingGameQuery request, CancellationToken cancellationToken)
    {
        var game = await _gameRepository.GetOne(request.GameId);

        if (game == null)
        {
            throw new GameNotFoundException($"Game {request.GameId} not found.");
        }

        var playingGame = game as GameDetail;

        if (playingGame.Host != request.UserName && playingGame.Guest != request.UserName)
        {
            throw new GameNotFoundException($"Game {request.GameId} not found.");
        }

        return playingGame;
    }
}

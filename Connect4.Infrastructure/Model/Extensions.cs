using System;
using Connect4.Common.Model;
using Microsoft.Extensions.Hosting;

namespace Connect4.Infrastructure.Model;

public static class Extensions
{
    public static Connect4Game ToDomain(this GameSqlite sqlite)
    {
        return new Connect4Game(
            sqlite.Id,
            sqlite.Name,
            sqlite.Host.Username,
            sqlite.Status switch
            {
                GameStatusSqlite.WaitingForPlayers => "WaitingForPlayers",
                GameStatusSqlite.InProgress => "InProgress",
                GameStatusSqlite.Finished => "Finished",
                _ => throw new InvalidOperationException($"Unknown game status: {sqlite.Status}")
            },
            //sqlite.Board.State,
            sqlite.Board.ToDomain(),
            sqlite.Guest?.Username,
            sqlite.Winner?.Username
        );
    }

    public static GameSqlite ToSqlite(
        this Connect4Game domain, 
        UserSqlite host, 
        UserSqlite? guest,
        UserSqlite? winner,
        BoardSqlite board
    )
    {
        return new GameSqlite
        {
            Id = domain.Id,
            Name = domain.Name,
            Host = host,
            Guest = guest,
            Winner = winner,
            Status = domain.Status switch
            {
                "WaitingForPlayers" => GameStatusSqlite.WaitingForPlayers,
                "InProgress" => GameStatusSqlite.InProgress,
                "Finished" => GameStatusSqlite.Finished,
                _ => throw new InvalidOperationException($"Unknown game status: {domain.Status}")
            },
            Board = board
        };
    }

    public static Board ToDomain(this BoardSqlite sqlite)
    {
        return new Board(
            sqlite.CurrentPlayer?.Username ?? string.Empty, // Handle null for CurrentPlayer
            sqlite.State
        );
    }

    public static BoardSqlite ToSqlite(this Board domain, UserSqlite host)
    {
        return new BoardSqlite
        {
            CurrentPlayer = host,
            State = domain.State
        };
    }

}

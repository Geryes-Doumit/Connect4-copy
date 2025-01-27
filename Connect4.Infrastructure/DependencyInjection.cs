using Microsoft.Extensions.DependencyInjection;
using Connect4.Domain.Repositories;
using Connect4.Infrastructure.Repositories;

namespace Connect4.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<UserQueryRepository, UserQueryRepositorySqlite>();
        services.AddScoped<GameQueryRepository, GameQueryRepositorySqlite>();
        services.AddScoped<GameRepository, GameRepositorySqlite>();
        services.AddScoped<BoardRepository, BoardRepositorySqlite>();
        services.AddScoped<BoardQueryRepository, BoardQueryRepositorySqlite>();

        return services;
    }
}


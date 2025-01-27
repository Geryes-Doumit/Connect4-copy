using Connect4.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Connect4.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddSingleton<ITokenBlacklistService, TokenBlacklistService>();
            services.AddTransient<IPlayerStatusService, PlayerStatusService>();
            services.AddTransient<IGameService, GameService>();

            return services;
        }
    }
}

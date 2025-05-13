
using Arzand.Modules.Cart.Services;
using Arzand.Shared.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Arzand.Modules.Cart.Extensions;

public static class CartModule
{
    public static IServiceCollection AddCartModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")!));

        services.AddCapWithPostgres(config);

        services.AddScoped<ICartService, CartService>();

        return services;
    }
}
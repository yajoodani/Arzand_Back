using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arzand.Shared.Infrastructure;

public static class CapSetup
{
    public static IServiceCollection AddCapWithPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCap(options =>
        {
            options.UsePostgreSql(configuration.GetConnectionString("EventBusConnection")!);
            options.UseRedis(configuration.GetConnectionString("Redis")!);
            //options.UseDashboard();

            options.FailedRetryCount = 5;
            options.FailedThresholdCallback = failed =>
            {
                Console.WriteLine($"CAP message failed after retries: {failed.MessageType}");
            };
        });

        return services;
    }
}

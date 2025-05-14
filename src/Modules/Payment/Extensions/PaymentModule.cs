using System;
using Arzand.Modules.Payment.Application;
using Arzand.Modules.Payment.Infrastructure;
using Arzand.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arzand.Modules.Payment.Extensions;

public static class PaymentModule
{
    public static IServiceCollection AddPaymentModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<PaymentDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("PaymentConnection")));

        services.AddCapWithPostgres(config);

        services.AddScoped<PaymentService>();

        return services;
    }
}
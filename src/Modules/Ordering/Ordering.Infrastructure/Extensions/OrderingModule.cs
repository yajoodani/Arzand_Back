using System;
using Arzand.Modules.Ordering.Application.Commands;
using Arzand.Modules.Ordering.Application.IntegrationEventHandlers;
using Arzand.Modules.Ordering.Application.Mappings;
using Arzand.Modules.Ordering.Domain.AggregatesModels.OrderAggregate;
using Arzand.Modules.Ordering.Infrastructure.Data;
using Arzand.Modules.Ordering.Infrastructure.Repositories;
using Arzand.Shared.Contracts.IntegrationEvents;
using Arzand.Shared.Domain;
using Arzand.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arzand.Modules.Ordering.Infrastructure.Extensions;

public static class OrderingModule
{
    public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration config)
    {
        // Application layer (MediatR, Validation)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));
        services.AddAutoMapper(typeof(OrderMappingProfile).Assembly);
        //services.AddValidatorsFromAssembly(typeof(SomeValidatorFromOrderingApplication).Assembly);

        // Infrastructure layer
        services.AddDbContext<OrderingDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("OrderingConnection")));

        services.AddCapWithPostgres(config);

        services.AddScoped<IUnitOfWork, OrderingDbContext>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddTransient<CartCheckedOutIntegrationEventHandler>();
        services.AddTransient<PaymentCompletedIntegrationEventHandler>();

        return services;
    }
}

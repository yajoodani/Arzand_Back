using System;
using Arzand.Modules.Catalog.Application.IntegrationEventHandlers;
using Arzand.Modules.Catalog.Application.Mappings;
using Arzand.Modules.Catalog.Application.Queries;
using Arzand.Modules.Catalog.Application.Services;
using Arzand.Modules.Catalog.Application.Validators;
using Arzand.Modules.Catalog.Domain.AggregatesModels.BrandAggregate;
using Arzand.Modules.Catalog.Domain.AggregatesModels.CategoryAggregate;
using Arzand.Modules.Catalog.Domain.AggregatesModels.ProductAggregate;
using Arzand.Modules.Catalog.Infrastructure.Data;
using Arzand.Modules.Catalog.Infrastructure.Repositories;
using Arzand.Shared.Contracts;
using Arzand.Shared.Domain;
using Arzand.Shared.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arzand.Modules.Catalog.Infrastructure.Extensions;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration config)
    {
        // Application layer (MediatR, Validation)
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllProductsQuery).Assembly));
        services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
        services.AddValidatorsFromAssembly(typeof(CreateProductCommandValidator).Assembly);

        // Infrastructure layer
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("CatalogConnection")));

        services.AddCapWithPostgres(config);

        services.AddScoped<IUnitOfWork, CatalogDbContext>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddTransient<OrderCanceledIntegrationEventHandler>();

        return services;
    }
}

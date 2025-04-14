using System;
using Arzand.Modules.Identity.Data;
using Arzand.Modules.Identity.Models;
using Arzand.Modules.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Arzand.Modules.Identity.Extensions;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration config)
    {
        // Add DbContext
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("IdentityConnection")));

        // Add Identity
        services.AddIdentityCore<AppUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        });

        services.AddScoped<AuthService>();

        return services;
    }
}

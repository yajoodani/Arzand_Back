using System.Security.Claims;
using System.Text;
using Arzand.Api.Middlewares;
using Arzand.Modules.Cart.Extensions;
using Arzand.Modules.Catalog.Application.Services;
using Arzand.Modules.Catalog.Infrastructure.Data;
using Arzand.Modules.Catalog.Infrastructure.Extensions;
using Arzand.Modules.Identity.Data;
using Arzand.Modules.Identity.Data.Seed;
using Arzand.Modules.Identity.Extensions;
using Arzand.Modules.Ordering.Infrastructure.Data;
using Arzand.Modules.Ordering.Infrastructure.Extensions;
using Arzand.Modules.Payment.Extensions;
using Arzand.Modules.Payment.Infrastructure;
using Arzand.Shared.Contracts;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your token in the text input below."

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            RoleClaimType = ClaimTypes.Role
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse(); // Stop the default logic
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"error\": \"Forbidden\"}");
            },
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = builder.Configuration.GetConnectionString("Redis")!;
    var conn = ConnectionMultiplexer.Connect(config!, options =>
    {
        options.AbortOnConnectFail = false;
    });

    conn.ConnectionFailed += (sender, args) =>
    {
        Console.WriteLine($"Redis Connection Failed: {args.Exception}");
    };

    conn.ConnectionRestored += (sender, args) =>
    {
        Console.WriteLine("Redis Connection Restored.");
    };

    return conn;
});

builder.Services.AddScoped<ICatalogService, CatalogService>();

builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddCartModule(builder.Configuration);
builder.Services.AddOrderingModule(builder.Configuration);
builder.Services.AddPaymentModule(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    void Migrate<TContext>() where TContext : DbContext
    {
        var db = services.GetRequiredService<TContext>();
        db.Database.Migrate();
    }

    try
    {
        Migrate<IdentityDbContext>();
        Migrate<CatalogDbContext>();
        Migrate<OrderingDbContext>();
        Migrate<PaymentDbContext>();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the databases.");
        throw;
    }
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthentication();
app.UseAuthorization();

// Debugging
app.Use(async (context, next) =>
{
    Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
    await next();
});

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentityDbSeeder.SeedRolesAsync(services);
}

app.Run();

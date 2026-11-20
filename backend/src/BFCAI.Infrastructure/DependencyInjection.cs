using BFCAI.Application.Common.Interfaces;
using BFCAI.Infrastructure.Caching;
using BFCAI.Infrastructure.Files;
using BFCAI.Infrastructure.Identity;
using BFCAI.Infrastructure.Payments;
using BFCAI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace BFCAI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Identity (users/roles stored in the same AspNet* tables as the MVC app)
        services.AddIdentityCore<Domain.Entities.ApplicationUser>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddSingleton<ITokenService, JwtTokenService>();

        // Files
        services.Configure<StorageOptions>(configuration.GetSection(StorageOptions.SectionName));
        services.AddSingleton<IFileStorage, LocalFileStorage>();

        // Cache (Redis)
        services.AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var redisOptions = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis") ?? "localhost:6379");
            redisOptions.AbortOnConnectFail = false;
            return ConnectionMultiplexer.Connect(redisOptions);
        });
        services.AddScoped<ICacheService, RedisCacheService>();

        // Payments (Stripe)
        services.Configure<StripeOptions>(configuration.GetSection(StripeOptions.SectionName));
        services.AddScoped<IPaymentGateway, StripePaymentGateway>();

        return services;
    }
}

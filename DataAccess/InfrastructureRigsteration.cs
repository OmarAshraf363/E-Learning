
using Banha_UniverCity.Data;
using Banha_UniverCity.Repository.IRepository;
using DataAccess.Repository.IRepository.Service;
using DataAccess.Repository.ModelsRepository;
using DataAccess.Repository.ModelsRepository.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DataAccess
{
    public static class InfrastructureRigsteration
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IvedioService, VedioService>();
            services.AddSingleton<IAssinmentService, AssinmentService>();
            services.AddSingleton<IImageService, ImageService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IStripeGetWay, StripeGetWay>();
            services.AddScoped<IPostService, PostService>();

            return services;
        }
   
        public static IServiceCollection RegisterCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configurationOptions = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configurationOptions);
            });
            services.AddScoped<ICacheService, CacheService>();
            return services;
        }
    }
}

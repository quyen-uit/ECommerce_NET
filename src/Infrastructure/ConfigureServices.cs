using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // add service for infrastructure
            services.AddDbContext<ApplicationDbContext>(options =>
                           options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                               builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // identity context
            services.AddDbContext<AppIdentityDbContext>(options =>
                          options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                               builder => builder.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName)));

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });

            // add identity service
            services.AddIdentityCore<AppUser>(options => { })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager<SignInManager<AppUser>>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService,PaymentService>();
            services.AddSingleton<IResponseCacheService,ResponseCacheService>();

            return services;
        }
    }
}

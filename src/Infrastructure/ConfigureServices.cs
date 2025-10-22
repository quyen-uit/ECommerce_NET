using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.Reposiories;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
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
                           options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                               builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var options = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis")!);
                return ConnectionMultiplexer.Connect(options);
            });

            // add identity service
            services.AddIdentity<AppUser, AppRole>(options => { })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();

            return services;
        }
    }
}

using API.Helpers;
using System.Reflection;

namespace API
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddAPIService(this IServiceCollection services)
        {
            // add service for web api
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure
{
    public static class StartupSetup
    {
        public static IApplicationBuilder Configure(this IApplicationBuilder app)
        {
            return app;
        }

        /// <summary>
        /// Add services and types that are infrastructure-related
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
        {
            return services;
        }
    }
}

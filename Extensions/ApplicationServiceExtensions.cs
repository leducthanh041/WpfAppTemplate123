using Microsoft.Extensions.DependencyInjection;
using WpfAppTemplate.Configs;

namespace WpfAppTemplate.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Register database
            services.AddSingleton<DatabaseConfig>();

            services.AddTransient<Views.MainWindow>();

            return services;
        }
    }
}

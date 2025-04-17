using Microsoft.Extensions.DependencyInjection;
using WpfAppTemplate.Configs;
using WpfAppTemplate.Helpers;
using WpfAppTemplate.Repositories;
using WpfAppTemplate.Services;
using WpfAppTemplate.ViewModels;
using WpfAppTemplate.Views;

namespace WpfAppTemplate.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Register database
            services.AddSingleton<DatabaseConfig>();

            // Đăng ký repositories và services
            services.AddScoped<IDaiLyService, DaiLyRepository>();
            services.AddScoped<ILoaiDaiLyService, LoaiDaiLyRepository>();
            services.AddScoped<IQuanService, QuanRepository>();

            // Register helpers
            services.AddSingleton<ComboBoxItemConverter>();

            // Đăng ký views
            services.AddTransient<Views.MainWindow>();
            services.AddTransient<Views.CapNhatDaiLyWindow>();
            services.AddTransient<Views.TiepNhanDaiLyWindow>();
            
            // Đăng ký viewmodels
            services.AddTransient<ViewModels.MainWindowViewModel>();
            services.AddTransient<ViewModels.CapNhatDaiLyViewModel>();
            services.AddTransient<ViewModels.TiepNhanDaiLyViewModel>();
            services.AddTransient<Func<int, CapNhatDaiLyViewModel>>(sp => dailyId =>
            new CapNhatDaiLyViewModel(
                sp.GetRequiredService<IDaiLyService>(),
                sp.GetRequiredService<IQuanService>(),
                sp.GetRequiredService<ILoaiDaiLyService>(),
                dailyId
            )
        );

            return services;
        }
    }
}

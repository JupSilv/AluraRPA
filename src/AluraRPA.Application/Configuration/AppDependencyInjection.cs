using AluraRPA.Application.Selenium.Controllers;
using AluraRPA.Application.Selenium.Pages;

namespace AluraRPA.Application.Configuration;
public static class AppDependencyInjection
{
    public static IServiceCollection AddAppDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        //! Alterar para repositorio verdadeiro
        services.AddTransient<IAluraRepository, AluraRepository>();

        services.AddSingleton<IProcessManagerService, ProcessManagerService>();

        services.AddSingleton<AluraController>();

        services.AddSingleton<HomePage>();

        services.AddSingleton<INavigator, Navigator>();


        services.AddSingleton<IDriverFactoryService>(_ =>
        {
            var driverFactory = new DriverFactoryService();
            AppDomain.CurrentDomain.ProcessExit += (_, _) => driverFactory?.Instance?.Quit();
            AppDomain.CurrentDomain.UnhandledException += (_, _) => driverFactory?.Instance?.Quit();
            return driverFactory;
        });
        return services;
    }
}
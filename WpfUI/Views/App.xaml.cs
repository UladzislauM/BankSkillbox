using Bank.Buisness;
using Microsoft.Extensions.DependencyInjection;
using NLog.Extensions.Logging;
using System.Windows;

namespace Bank
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ServiceCollection Services = new ServiceCollection();
            Services = ConfigureServices(Services);

            var serviceProvider = Services.BuildServiceProvider();

            var viewModel = serviceProvider.GetRequiredService<MainBankViewModel>();

            var window = new MainWindow
            {
                DataContext = viewModel
            };

            window.Show();
        }

        public ServiceCollection ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<MainBankViewModel>();
            services.AddSingleton<ErrorBankViewModel>();
            services.AddSingleton<Service>();
            services.AddSingleton<RepositoryForDB>();
            services.AddSingleton<RepositoryForJson>();
            services.AddSingleton<IDialogService, DefaultDialogService>();
            services.AddLogging(builder =>
            {
                builder.AddNLog();
            });
            return services;
        }

    }
}

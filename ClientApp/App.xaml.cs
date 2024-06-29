using System;
using System.Windows;
using ClientApp.Service;
using ClientApp.Networking;
using ClientApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp
{
    public partial class App : Application
    {
        private Client _client = null;
        private ServiceProvider _serviceProvider;
        protected void OnStartup(object sender, StartupEventArgs e)
        {
          
            _client = new Client();

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<IClient, Client>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<MainWindow>();
            services.AddTransient<ShoppingViewModel>();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _client?.Dispose();
            base.OnExit(e);
        }
    }
}


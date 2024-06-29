using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerApp.Controller;
using ServerApp.Entity;
using ServerApp.Networking;
using ServerApp.Service;

namespace ServerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (context, config) =>
                    {
                        config.AddConfiguration(builder.Build());
                    }
                )
                .ConfigureServices(
                    (context, services) =>
                    {
                        services.AddDbContext<northwindContext>(options =>
                            options.UseSqlServer(
                                context.Configuration.GetConnectionString("DefaultConnection")
                            )
                        );
                        services.AddHostedService<ServerService>();
                        services.AddTransient<IProductService , ProductService>();
                        services.AddTransient<RoutingHandler>();
                        services.AddSingleton<Server>();
                        services.AddTransient<OrderController>();
                        services.AddTransient<ProductController>();
                    }
                )
                .Build();
            host.Run();
        }
        public static void BuildConfig(IConfigurationBuilder builder)
        {
            builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }


    }
}

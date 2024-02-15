using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;
using WPInventory.Data;
using WPInventory.Worker.BackgroundService;
using WPInventory.Worker.BackgroundService.ConnectionManager;
using WPInventory.Worker.BackgroundService.PropCreators;
using WPInventory.Worker.BackgroundService.Services;

namespace WPInventory.Worker
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                Log.Information("Starting GenericHost");

                var host = new HostBuilder()
                    .ConfigureHostConfiguration(configHost =>
                    {
                        configHost.SetBasePath(Directory.GetCurrentDirectory());
                        configHost.AddJsonFile("appsettings.json", optional: false);
                        configHost.AddCommandLine(args);
                        configHost.AddEnvironmentVariables();

                    })
                    .ConfigureAppConfiguration((hostContext, configHost) =>
                    {
                        configHost.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    })
                    .ConfigureServices((hostContext, services) =>
                    {
                        var configuration = hostContext.Configuration;
                        services.AddSingleton<IWMIConnectionManager, SimpleWMIConnectionManager>();
                        services.AddSingleton<ICompInfoService, CompInfoService>();
                        services.AddSingleton<IComputerAnalyzer, ComputerAnalyzer>();
                        services.AddHostedService<CompsInfoBackGroundService>();
                        var dbOptionsBuilder = new DbContextOptionsBuilder<ComputerInfoContext>().UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                        var connectionString = configuration.GetConnectionString("DefaultConnection");
                        services.AddDbContext<ComputerInfoContext>(options =>
                            options.UseSqlServer(connectionString),ServiceLifetime.Transient);
                        services.AddSingleton(dbOptionsBuilder.Options);
                    })

                    .UseConsoleLifetime()
                    .ConfigureLogging(builder => builder
                        .AddDebug())
                    .UseSerilog((ctx, config) => { config.ReadFrom.Configuration(ctx.Configuration); })
                    .Build();
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "GenericHost terminated unexpectedly");
            }
            finally
            {
                Log.Information("Stopping GenericHost");
                Log.CloseAndFlush();
            }

        }
    }
}

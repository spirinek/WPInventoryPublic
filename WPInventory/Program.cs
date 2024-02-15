using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using WPInventory.Initialization;

namespace WPInventory
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Console.Title = "WPInventory";
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Debug("init main");
                CreateWebHostBuilder(args).Build()
                    .DataInitialize()
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>().ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Information);
                }).UseSerilog((ctx, config) => { config.ReadFrom.Configuration(ctx.Configuration); });
}
    }

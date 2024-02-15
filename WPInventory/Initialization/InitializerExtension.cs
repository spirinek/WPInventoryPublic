using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WPInventory.Data.DataSeed;

namespace WPInventory.Initialization
{
   public static class InitializerExtension
    {
        public static IWebHost DataInitialize(this IWebHost host, bool useMigrate = true)
        {
            using (var scope = host.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var initializer = ActivatorUtilities.CreateInstance<ComputerContextInitializer>(scope.ServiceProvider);
                initializer.Initialize(useMigrate).GetAwaiter().GetResult();
            }

            return host;
        }
    }
}

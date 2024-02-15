using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPInventory.Data;
using WPInventory.Worker.BackgroundService.ConnectionManager;
using WPInventory.Worker.BackgroundService.Services;

namespace WPInventory.Worker.BackgroundService.PropCreators
{
    public class CompInfoService : ICompInfoService
    {
        private readonly ComputerInfoContext _dbContext;
        private readonly IComputerAnalyzer _computerAnalyzer;
        private readonly IWMIConnectionManager _connectionManager;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _commputerServiceLogger;
        private readonly ILogger _builderLogger;
        private readonly ILogger _adLogger;

        public CompInfoService(ILoggerFactory loggerFactory, ComputerInfoContext dbContext, IComputerAnalyzer computerAnalyzer, IWMIConnectionManager connectionManager)
        {
            _dbContext = dbContext;
            _computerAnalyzer = computerAnalyzer;
            _connectionManager = connectionManager;
            _loggerFactory = loggerFactory;
            _commputerServiceLogger = _loggerFactory.CreateLogger(nameof(CompInfoService));
            _builderLogger = _loggerFactory.CreateLogger("ADComputerBuilder");
            _adLogger = _loggerFactory.CreateLogger("ADlogger");
        }

        public async Task CreateInfoAsync()
        {
            var adLdapStrings = _dbContext.AdScopes.Where(x => x.IsEnabled).Select(x => x.ScopePath).ToList();
            var compsInAd = ADCompListCreator.GetADComputers(adLdapStrings, _adLogger);

            _commputerServiceLogger.LogInformation("Сбор информации начат");

            var tasks = new List<Task>();

            foreach (var comp in compsInAd)
            {
                tasks.Add(  Task.Run(async () =>
                {
                    var options = _connectionManager.GetOptions();
                    var builder = new ComputerBuilder(comp, options, _builderLogger);
                    var computer = await builder.Build();
                    if (computer != null)
                    {
                        await _computerAnalyzer.AddOrUpdateComputerInDatabase(computer);
                    }
                }));
            }

            await Task.WhenAll(tasks);
            _commputerServiceLogger.LogInformation("Сбор инфорации завершён");
        }
    }
}

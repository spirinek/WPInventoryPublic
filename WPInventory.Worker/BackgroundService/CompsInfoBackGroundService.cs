using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WPInventory.Worker.BackgroundService.Services;

namespace WPInventory.Worker.BackgroundService
{
    public class CompsInfoBackGroundService : IHostedService
    {

        private readonly ILogger _logger;
        private readonly ICompInfoService _compInfoService;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public CompsInfoBackGroundService(ILogger<CompsInfoBackGroundService> logger, ICompInfoService compInfoService, IHostApplicationLifetime applicationLifetime)
        {
            _logger = logger;
            _compInfoService = compInfoService;
            _applicationLifetime = applicationLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            DoWorkAsync();
            return Task.CompletedTask;
        }
        public async void DoWorkAsync()
        {
            _logger.LogInformation($"Starting {nameof(CompsInfoBackGroundService)}");
            await _compInfoService.CreateInfoAsync();
            await Task.Run(() => _applicationLifetime.StopApplication());
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {nameof(CompsInfoBackGroundService)}");
            return Task.CompletedTask;
        }
    }
}

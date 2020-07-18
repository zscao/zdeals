using MassTransit;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZDeals.Engine
{
    class TaskMan : BackgroundService
    {
        public readonly IBusControl _bus;
        public readonly ILogger<TaskMan> _logger;

        private int _count = 0;

        public TaskMan(IBusControl bus, ILogger<TaskMan> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting TaskCenter ...");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("TaskCenter is running.");
                await _bus.Send(new Message.Commands.CrawlPage
                {
                    Url = "https://www.centrecom.com.au/clearance" //  $"http://localhost/deals/{_count ++}"
                });
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}

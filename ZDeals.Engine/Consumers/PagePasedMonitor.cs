using MassTransit;

using Microsoft.Extensions.Logging;

using System.Text.Json;
using System.Threading.Tasks;

using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Consumers
{
    class PagePasedMonitor : IConsumer<PageParsed>
    {
        private readonly ILogger<PagePasedMonitor> _logger;
        public PagePasedMonitor(ILogger<PagePasedMonitor> logger)
        {
            _logger = logger;

            _logger.LogInformation("PageParseMonitor created.");
        }

        public Task Consume(ConsumeContext<PageParsed> context)
        {
            var message = context.Message;

            _logger.LogInformation($"Received PageParsed event: {message.Uri} {message.ParsedTime}");
            _logger.LogInformation(JsonSerializer.Serialize(message.Product));

            return Task.CompletedTask;
        }
    }
}

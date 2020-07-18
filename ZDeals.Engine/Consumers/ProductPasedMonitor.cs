using MassTransit;

using Microsoft.Extensions.Logging;

using System.Text.Json;
using System.Threading.Tasks;

using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Consumers
{
    class ProductPasedMonitor : IConsumer<ProductParsed>
    {
        private readonly ILogger<ProductPasedMonitor> _logger;
        public ProductPasedMonitor(ILogger<ProductPasedMonitor> logger)
        {
            _logger = logger;

            _logger.LogInformation("PageParseMonitor created.");
        }

        public Task Consume(ConsumeContext<ProductParsed> context)
        {
            var message = context.Message;

            _logger.LogInformation($"Received PageParsed event: {message.Uri} {message.ParsedTime}");
            _logger.LogInformation(JsonSerializer.Serialize(message.Product));

            return Task.CompletedTask;
        }
    }
}

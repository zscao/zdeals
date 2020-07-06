using MassTransit;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;
using ZDeals.Engine.Message.Commands;
using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Parser.Consumers
{

    class TestPageParser : IConsumer<ParsePage>
    {
        private readonly ILogger<TestPageParser> _logger;

        public TestPageParser(ILogger<TestPageParser> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<ParsePage> context)
        {
            var message = context.Message;

            _logger.LogInformation($"Received ParsePage command: {message.Url}");

            return context.Publish(new PageParsed
            {
                Url = message.Url,
                ParseTime = DateTimeOffset.Now
            });
        }
    }
}

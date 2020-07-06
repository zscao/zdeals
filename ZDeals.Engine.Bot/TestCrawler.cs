﻿using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Core;

namespace ZDeals.Engine.Bot
{
    class TestCrawler : ICrawler
    {
        private readonly ILogger<TestCrawler> _logger;

        public event EventHandler<PageParsedEventArgs> PageParsed;

        public TestCrawler(ILogger<TestCrawler> logger)
        {
            _logger = logger;
        }
        public Task StartCrawling(string url, CancellationTokenSource cts)
        {
            _logger.LogInformation($"Start crawling {url}");
            return Task.Delay(new TimeSpan(0, 0, 5), cts.Token);
        }
    }
}
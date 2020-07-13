using Abot2.Crawler;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Core;

namespace ZDeals.Engine.Crawlers.Scorptec
{
    public class ProductFeedCrawler: ICrawler
    {
        private readonly ILogger<ProductFeedCrawler> _logger;

        public ProductFeedCrawler(ILogger<ProductFeedCrawler> logger)
        {
            _logger = logger;

        }

        public event EventHandler<PageParsedEventArgs> PageParsed;

        public async Task StartCrawling(string startUrl, CancellationTokenSource cts)
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 5,
                MinCrawlDelayPerDomainMilliSeconds = 3000
            };

            try
            {
                var hyperLinkParser = new ProductFeedHyperLinkParser(_logger);
                var crawler = new PoliteWebCrawler(config, null, null, null, null, hyperLinkParser, null, null, null);

                crawler.PageCrawlCompleted += Crawler_PageCrawlCompleted;

                var result = await crawler.CrawlAsync(new Uri(startUrl), cts);

                _logger.LogInformation($"Complted Scorptec. Time elapsed: {result.Elapsed}s.");
            }
            catch(OperationCanceledException ex)
            {
                _logger.LogError("Operation canceled exception caught in StartCrawling.", ex);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error when crawling {startUrl}", ex);
            }
        }

        private void Crawler_PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var page = e.CrawledPage;
            _logger.LogInformation("Status: {status}, Url: {url}", page.HttpResponseMessage.StatusCode, page.Uri);

            if (page.Uri.LocalPath.EndsWith(".xml")) return;

            var productParser = new ProductParser();
            var product = productParser.Parse(page.AngleSharpHtmlDocument, page.Uri);

            if(product != null)
            {
                PageParsed?.Invoke(this, new PageParsedEventArgs
                {
                    PageUri = e.CrawledPage.Uri,
                    Product = product
                });
            }

        }
    }
}

using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Core;
using ZDeals.Engine.Schedulers;

namespace ZDeals.Engine.Crawlers.CentreCom
{
    public class ClearancePageCrawler: ICrawler
    {
        private readonly IScheduler _scheduler;
        private readonly ILogger<ClearancePageCrawler> _logger;

        public ClearancePageCrawler(ISiteScheduler scheduler, ILogger<ClearancePageCrawler> logger)
        {
            scheduler.SiteCode = "www.centrecom.com.au";

            _scheduler = scheduler;
            _logger = logger;
        }

        public event EventHandler<PageParsedEventArgs> PageParsed;

        public async Task StartCrawling(string startUrl, CancellationTokenSource cancellationTokenSource)
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 1000,
                MinCrawlDelayPerDomainMilliSeconds = 3000
            };
            
            try
            {
                var hyperLinkParser = new ClearancePageHyperLinkParser();
                var crawler = new PoliteWebCrawler(config, null, null, _scheduler, null, hyperLinkParser, null, null, null);
                crawler.PageCrawlCompleted += Crawler_PageCrawlCompleted;

                var result = await crawler.CrawlAsync(new Uri(startUrl), cancellationTokenSource);

                _logger.LogInformation($"Completed CentreCom. Time elapsed: {result.Elapsed}s.");
            }
            catch(OperationCanceledException ex)
            {
                _logger.LogError("Operation canceled exception caught in StartCrawling.", ex);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error when crawling {startUrl}.", ex);
            }
        }

        private void Crawler_PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            _logger.LogInformation("Status: {status}  Url: {url}", e.CrawledPage.HttpResponseMessage.StatusCode, e.CrawledPage.Uri);

            var productParser = new ProductParser();
            var product = productParser.Parse(e.CrawledPage.AngleSharpHtmlDocument, e.CrawledPage.Uri);
            if (product != null)
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

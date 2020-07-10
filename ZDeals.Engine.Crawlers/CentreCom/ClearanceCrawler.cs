using Abot2.Crawler;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Core;

namespace ZDeals.Engine.Crawlers.CentreCom
{
    public class ClearanceCrawler: ICrawler
    {
        private readonly WebCrawler _crawler;
        private readonly ILogger<ClearanceCrawler> _logger;

        private bool _foundProduct = false;

        public ClearanceCrawler(ILogger<ClearanceCrawler> logger)
        {
            _logger = logger;

            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 10_000,
                MinCrawlDelayPerDomainMilliSeconds = 3000
            };

            var hyperLinkParser = new ClearanceHyperLinkParser();
            _crawler = new PoliteWebCrawler(config, null, null, null, null, hyperLinkParser, null, null, null);
            _crawler.PageCrawlCompleted += Crawler_PageCrawlCompleted;
            _crawler.ShouldCrawlPageDecisionMaker = ShouldCrawlPage;
        }

        public event EventHandler<PageParsedEventArgs> PageParsed;

        public async Task StartCrawling(string startUrl, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                var result = await _crawler.CrawlAsync(new Uri(startUrl), cancellationTokenSource);
                _logger.LogInformation($"Completed CentreCom. Time elapsed: {result.Elapsed}s.");
            }
            catch(OperationCanceledException ex)
            {
                _logger.LogError("Operation canceled exception caught in StartCrawling.", ex);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error when crawling.", ex);
            }
        }

        private void Crawler_PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            _logger.LogInformation("Status: {status}  Url: {url}", e.CrawledPage.HttpResponseMessage.StatusCode, e.CrawledPage.Uri);

            var pageParser = new ProductParser();
            var product = pageParser.Parse(e.CrawledPage.AngleSharpHtmlDocument, e.CrawledPage.Uri);
            if (product != null)
            {
                _foundProduct = true;

                PageParsed?.Invoke(this, new PageParsedEventArgs
                {
                    PageUri = e.CrawledPage.Uri,
                    Product = product
                });
            }
        }

        private CrawlDecision ShouldCrawlPage(PageToCrawl page, CrawlContext context)
        {
            return new CrawlDecision { Allow = !_foundProduct, Reason = "Found one", ShouldStopCrawl = _foundProduct,  };
        }
    }


}

using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Core;

namespace ZDeals.Engine.Crawlers.CentreCom
{
    public class ClearancePageCrawler: ICrawler
    { 
        private readonly ICrawlDecisionMaker _crawlDecisionMaker;
        private readonly ILogger<ClearancePageCrawler> _logger;

        public ClearancePageCrawler(ICrawlDecisionMaker crawlDecisionMaker, ILogger<ClearancePageCrawler> logger)
        {
            _crawlDecisionMaker = crawlDecisionMaker;
            _logger = logger;
        }

        public event EventHandler<PageCrawledEventArgs> OnPageCrawled;

        public async Task StartCrawling(string startUrl, CancellationTokenSource cancellationTokenSource)
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 100,
                MinCrawlDelayPerDomainMilliSeconds = 3000
            };
            
            try
            {
                var hyperLinkParser = new ClearancePageHyperLinkParser();

                var crawler = new PoliteWebCrawler(config, _crawlDecisionMaker, null, null, null, hyperLinkParser, null, null, null);
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
            _logger.LogInformation("Status: {status}  Url: {url}", e.CrawledPage.HttpResponseMessage?.StatusCode, e.CrawledPage.Uri);

            if(e.CrawledPage.HttpResponseMessage?.StatusCode == System.Net.HttpStatusCode.OK && e.CrawledPage != null)
            {
                var page = e.CrawledPage;
                
                OnPageCrawled?.Invoke(this, new PageCrawledEventArgs
                {
                    Page = new Message.Events.PageCrawled
                    {
                        Uri = page.Uri,
                        ParentUri = page.ParentUri,
                        CrawledTime = DateTime.Now,
                        Content = new Message.Events.PageContent
                        {
                            Charset = page.Content?.Charset,
                            Text = page.Content?.Text
                        },
                        IsRoot = page.IsRoot
                    }
                });
            }
        }
    }


}

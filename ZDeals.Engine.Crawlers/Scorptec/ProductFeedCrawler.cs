using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Core;

namespace ZDeals.Engine.Crawlers.Scorptec
{
    public class ProductFeedCrawler: IPageCrawler
    {
        private readonly ICrawlDecisionMaker _crawlDecisionMaker;
        private readonly ILogger<ProductFeedCrawler> _logger;

        public ProductFeedCrawler(ICrawlDecisionMaker crawlDecisionMaker, ILogger<ProductFeedCrawler> logger)
        {
            _crawlDecisionMaker = crawlDecisionMaker;
            _logger = logger;

        }

        public event EventHandler<PageCrawledEventArgs> OnPageCrawled;

        public async Task StartCrawling(string startUrl, string store, CancellationTokenSource cts)
        {
            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 20,
                MinCrawlDelayPerDomainMilliSeconds = 3000
            };

            try
            {
                var hyperLinkParser = new ProductFeedHyperLinkParser(_logger);
                var crawler = new PoliteWebCrawler(config, _crawlDecisionMaker, null, null, null, hyperLinkParser, null, null, null);

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
            _logger.LogInformation("Status: {status}  Url: {url}", e.CrawledPage.HttpResponseMessage?.StatusCode, e.CrawledPage.Uri);

            if (e.CrawledPage.HttpResponseMessage?.StatusCode == System.Net.HttpStatusCode.OK && e.CrawledPage != null)
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

            //if (page.Uri.LocalPath.EndsWith(".xml")) return;

            //var productParser = new ProductParser();
            //var product = productParser.Parse(page.AngleSharpHtmlDocument, page.Uri);

            //if(product != null)
            //{
            //    PageParsed?.Invoke(this, new PageCrawledEventArgs
            //    {
            //        PageUri = e.CrawledPage.Uri,
            //        Product = product
            //    });
            //}

        }
    }
}

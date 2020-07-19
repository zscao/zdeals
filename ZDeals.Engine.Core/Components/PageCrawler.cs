using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZDeals.Engine.Core.Components
{
    public class PageCrawler: IPageCrawler
    {
        private readonly ICrawlDecisionMaker _crawlDecisionMaker;
        private readonly IStoreScheduler _scheduler;
        private readonly ILogger<PageCrawler> _logger;

        protected IHtmlParser HtmlParser { get; set; }

        public PageCrawler(ICrawlDecisionMaker crawlDecisionMaker, IStoreScheduler scheduler, ILogger<PageCrawler> logger)
        {
            _crawlDecisionMaker = crawlDecisionMaker;
            _scheduler = scheduler;
            _logger = logger;
        }

        public event EventHandler<PageCrawledEventArgs> OnPageCrawled;

        public async Task StartCrawling(CrawlerOption option, CancellationTokenSource cancellationTokenSource)
        {
            if (option.MinCrawlDelaySeconds < 1) option.MinCrawlDelaySeconds = 1;

            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = option.MaxPagesToCrawl,
                MinCrawlDelayPerDomainMilliSeconds = option.MinCrawlDelaySeconds * 1000
            };

            try
            {
                _scheduler.AddTrackedPages(option.Store);

                var urls = option.StartUrl.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var url in urls)
                {
                    if (string.IsNullOrWhiteSpace(url)) continue;

                    cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    // do work
                    var crawler = new PoliteWebCrawler(config, _crawlDecisionMaker, null, _scheduler, null, HtmlParser, null, null, null);
                    crawler.PageCrawlCompleted += Crawler_PageCrawlCompleted;

                    var result = await crawler.CrawlAsync(new Uri(url), cancellationTokenSource);

                    _logger.LogInformation($"Completed {url}. Time elapsed: {result.Elapsed}s.");
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError("Operation canceled exception caught in StartCrawling.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when crawling {option.StartUrl}.", ex);
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
        }
    }
}

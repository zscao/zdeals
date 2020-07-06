using Abot2.Core;
using Abot2.Crawler;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using ZDeals.Engine.Core;

namespace ZDeals.Engine.Crawlers.CentreCom
{
    public class CentreComCrawler: ICrawler
    {
        private readonly WebCrawler _crawler;
        private readonly ILogger<CentreComCrawler> _logger;

        public CentreComCrawler(ILogger<CentreComCrawler> logger)
        {
            _logger = logger;

            var config = new CrawlConfiguration
            {
                MaxPagesToCrawl = 10,
                MinCrawlDelayPerDomainMilliSeconds = 3000
            };

            var hyperLinkParser = new CentreComHyperLinkParser();
            _crawler = new PoliteWebCrawler(config, null, null, null, null, hyperLinkParser, null, null, null);
            _crawler.PageCrawlCompleted += Crawler_PageCrawlCompleted;
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
                _logger.LogError("Operation canceled exception caught in StartCrawling.");
            }
            catch(Exception ex)
            {
                _logger.LogError("Error when crawling.", ex);
            }
        }

        private void Crawler_PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            _logger.LogInformation("Status: {status}  Url: {url}", e.CrawledPage.HttpResponseMessage.StatusCode, e.CrawledPage.Uri);

            PageParsed?.Invoke(this, new PageParsedEventArgs
            {
                PageUri = e.CrawledPage.Uri
            });
        }
    }

    class CentreComHyperLinkParser : IHtmlParser
    {
        public IEnumerable<HyperLink> GetLinks(CrawledPage crawledPage)
        {
            var result = new List<HyperLink>();

            var document = crawledPage.AngleSharpHtmlDocument;
            var products = document.QuerySelectorAll("a.prbox_link[href]");

            foreach (var p in products)
            {
                var url = p.GetAttribute("href");
                if (!string.IsNullOrEmpty(url))
                {
                    result.Add(new HyperLink { HrefValue = MakeUri(crawledPage.ParentUri, url) });
                }
            }

            return result;
        }

        private Uri MakeUri(Uri parentUri, string href)
        {
            if (href.StartsWith("http://") || href.StartsWith("https://"))
                return new Uri(href);
            else if (href.StartsWith("//"))
                return new Uri($"{parentUri.Scheme}:{href}");
            else if (href.StartsWith("/"))
                return new Uri(parentUri.GetLeftPart(UriPartial.Authority) + href);
            else
                return new Uri(parentUri, href);
        }
    }
}

using System;

namespace ZDeals.Engine.Core
{
    public class CrawlerOption
    {
        public CrawlerOption(string startUrl, string store, TimeSpan timeout, int pagesToCrawl, int crawlDelay)
        {
            this.StartUrl = startUrl;
            this.Store = store;
            this.Timeout = timeout;
            this.MaxPagesToCrawl = pagesToCrawl;
            this.MinCrawlDelaySeconds = crawlDelay;
        }

        public string Store { get; set; }

        public string StartUrl { get; set; }
        public TimeSpan Timeout { get; set; }

        public int MaxPagesToCrawl { get; set; }
        public int MinCrawlDelaySeconds { get; set; }
    }
}

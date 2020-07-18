using System;

namespace ZDeals.Engine.Core
{
    public class CrawlerOption<T> where T: IPageCrawler
    {
        public CrawlerOption()
        {

        }

        public CrawlerOption(string startUrl, string store, TimeSpan timeout)
        {
            this.StartUrl = startUrl;
            this.Store = store;
            this.Timeout = timeout;
        }

        public string Store { get; set; }

        public string StartUrl { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}

using System;

namespace ZDeals.Engine.Core
{
    public class CrawlerOption<T> where T: ICrawler
    {
        public CrawlerOption()
        {

        }

        public CrawlerOption(string startUrl, TimeSpan timeout)
        {
            this.StartUrl = startUrl;
            this.Timeout = timeout;
        }

        public string StartUrl { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}

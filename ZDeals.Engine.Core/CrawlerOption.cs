using System;

namespace ZDeals.Engine.Core
{
    public class CrawlerOption<T> where T: ICrawler
    {
        public string StartUrl { get; set; }
        public TimeSpan Timeout { get; set; }
    }
}

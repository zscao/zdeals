using System;

using ZDeals.Engine.Core;

namespace ZDeals.Engine.Bot
{
    public class GenericCrawlerOption<T> : CrawlerOption where T : IPageCrawler
    {
        public GenericCrawlerOption(string startUrl, string store, TimeSpan timeout, int pages, int delays) : base(startUrl, store, timeout, pages, delays)
        {
        }
    }
}

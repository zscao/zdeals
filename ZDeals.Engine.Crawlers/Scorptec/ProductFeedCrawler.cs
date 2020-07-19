using Abot2.Core;

using Microsoft.Extensions.Logging;

using ZDeals.Engine.Core;
using ZDeals.Engine.Core.Components;

namespace ZDeals.Engine.Crawlers.Scorptec
{
    public class ProductFeedCrawler: PageCrawler
    {
        public ProductFeedCrawler(ICrawlDecisionMaker crawlDecisionMaker, IStoreScheduler scheduler, ILogger<ProductFeedCrawler> logger)
            : base(crawlDecisionMaker, scheduler, logger)
        {
            HtmlParser = new ProductFeedHyperLinkParser(logger);
        }

    }
}

using Abot2.Core;

using Microsoft.Extensions.Logging;

using ZDeals.Engine.Core;
using ZDeals.Engine.Core.Components;

namespace ZDeals.Engine.Crawlers.CentreCom
{
    public class ClearancePageCrawler: PageCrawler
    {
        public ClearancePageCrawler(ICrawlDecisionMaker crawlDecisionMaker, IStoreScheduler scheduler, ILogger<ClearancePageCrawler> logger)
            : base(crawlDecisionMaker, scheduler, logger)
        {
            HtmlParser = new ClearancePageHyperLinkParser();
        }
    }


}

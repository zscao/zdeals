using Abot2.Core;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System.Linq;

using ZDeals.Engine.Data;
using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Crawlers.DecisionMakers
{
    public class VisitedDecisionMaker : CrawlDecisionMaker
    {
        private readonly EngineDbContext _dbContext;
        private readonly ILogger<VisitedDecisionMaker> _logger;

        public VisitedDecisionMaker(EngineDbContext dbContext, ILogger<VisitedDecisionMaker> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public override CrawlDecision ShouldCrawlPage(PageToCrawl pageToCrawl, CrawlContext crawlContext)
        {
            var decision = base.ShouldCrawlPage(pageToCrawl, crawlContext);
            if (decision.Allow == false) return decision;

            _logger.LogWarning($"Checking page {pageToCrawl.Uri}");

            var visited = _dbContext.VisitedPages.FirstOrDefault(x => x.Url == pageToCrawl.Uri.AbsoluteUri);
            if(visited != null)
            {
                if (visited.ContentType != VisitedPageContentType.Index)
                {
                    decision.Allow = false;
                    decision.Reason = "Visited";
                }
            }
            return decision;
        }
    }
}

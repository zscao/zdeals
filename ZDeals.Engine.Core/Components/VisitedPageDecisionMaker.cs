using Abot2.Core;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Linq;

using ZDeals.Engine.Data;
using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Core.Components
{
    public class VisitedPageDecisionMaker : CrawlDecisionMaker
    {
        private readonly EngineDbContext _dbContext;
        private readonly ILogger<VisitedPageDecisionMaker> _logger;

        public VisitedPageDecisionMaker(EngineDbContext dbContext, ILogger<VisitedPageDecisionMaker> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public override CrawlDecision ShouldCrawlPage(PageToCrawl pageToCrawl, CrawlContext crawlContext)
        {
            var decision = base.ShouldCrawlPage(pageToCrawl, crawlContext);
            if (decision.Allow == false) return decision;

            _logger.LogDebug($"Checking page {pageToCrawl.Uri}");

            var visited = _dbContext.VisitedPages.FirstOrDefault(x => x.Url == pageToCrawl.Uri.AbsoluteUri);
            if(visited != null)
            {
                if(visited.ContentType == VisitedPageContentType.Unknown || (visited.ContentType == VisitedPageContentType.Product && visited.LastVisitedTime.AddDays(1) < DateTime.Today))
                {
                    decision.Allow = false;
                    decision.Reason = "Visited";
                }
            }
            return decision;
        }
    }
}

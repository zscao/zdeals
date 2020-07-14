using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;

using ZDeals.Engine.Data;

namespace ZDeals.Engine.Schedulers.Repo
{
    public class ScheduledPageRepo : IScheduledPageRepo
    {
        private readonly EngineDbContext _dbContext;
        private readonly ILogger<ScheduledPageRepo> _logger;

        public ScheduledPageRepo(EngineDbContext dbContext, ILogger<ScheduledPageRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public bool IsVisitedUri(Uri uri)
        {
            _logger.LogWarning($"Checking known URI in DB: {uri}");
            return _dbContext.Products.AsNoTracking().Any(x => x.Url == uri.AbsoluteUri);
        }
    }
}

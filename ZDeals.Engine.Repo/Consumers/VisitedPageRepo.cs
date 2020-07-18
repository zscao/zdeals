using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Threading.Tasks;

using ZDeals.Engine.Data;
using ZDeals.Engine.Data.Entities;
using ZDeals.Engine.Message.Events;

namespace ZDeals.Engine.Repo.Consumers
{
    class VisitedPageRepo: IConsumer<PageVisited>
    {
        private readonly EngineDbContext _dbContext;
        private readonly ILogger<ProductRepo> _logger;

        public VisitedPageRepo(EngineDbContext dbContext, ILogger<ProductRepo> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<PageVisited> context)
        {
            var page = context.Message;

            _logger.LogInformation($"Received PageVisited {page.Uri}, type: {page.ContentType}");

            try
            {
                var visited = await _dbContext.VisitedPages.FirstOrDefaultAsync(x => x.Url == page.Uri.AbsoluteUri);
                if(visited != null)
                {
                    // change unknown to known type
                    if (visited.ContentType == VisitedPageContentType.Unknown && page.ContentType != VisitedPageContentType.Unknown)
                    {
                        visited.ContentType = page.ContentType;
                        await _dbContext.SaveChangesAsync();
                    }
                    // change index to product
                    else if (visited.ContentType == VisitedPageContentType.Index && page.ContentType == VisitedPageContentType.Product)
                    {
                        visited.ContentType = page.ContentType;
                        await _dbContext.SaveChangesAsync();
                    }
                    return;
                }

                // save new
                var entry = _dbContext.VisitedPages.Add(new VisitedPageEntity
                {
                    Url = page.Uri.AbsoluteUri,
                    ContentType = page.ContentType,
                    LastVisitedTime = page.VisitedTime
                });

                // save parent
                var parent = await _dbContext.VisitedPages.FirstOrDefaultAsync(x => x.Url == page.ParentUri.AbsoluteUri);
                if(parent != null)
                {
                    if (parent.ContentType == VisitedPageContentType.Unknown) parent.ContentType = VisitedPageContentType.Index;
                }
                else
                {
                    _dbContext.VisitedPages.Add(new VisitedPageEntity
                    {
                        Url = page.ParentUri.AbsoluteUri,
                        ContentType = VisitedPageContentType.Index,
                        LastVisitedTime = page.VisitedTime
                    });
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save visited page {page.Uri}.", ex);
            }
        }
    }
}

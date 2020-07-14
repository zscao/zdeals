using Abot2.Core;
using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;

using ZDeals.Engine.Schedulers.Repo;

namespace ZDeals.Engine.Schedulers.Impl
{
    public class MySqlScheduler : ISiteScheduler
    {        
        private readonly IPagesToCrawlRepository _pagesToCrawlRepo;
        private readonly ICrawledUrlRepository _crawledUrlRepo;
        private readonly IScheduledPageRepo _scheduledPageRepo;

        private readonly ILogger<MySqlScheduler> _logger;

        public string SiteCode { get; set; } = "default";

        public MySqlScheduler(IPagesToCrawlRepository pagesToCrawlRepository, 
            ICrawledUrlRepository crawledUrlRepository, 
            IScheduledPageRepo scheduledPageRepo, 
            ILogger<MySqlScheduler> logger)
        {
            _pagesToCrawlRepo = pagesToCrawlRepository;
            _crawledUrlRepo = crawledUrlRepository;
            _scheduledPageRepo = scheduledPageRepo;
            _logger = logger;

            _logger.LogDebug($"SiteScheduler constructed");
        }

        public int Count => _pagesToCrawlRepo.Count();

        public void Add(PageToCrawl page)
        {
            if (page == null) throw new ArgumentNullException("page");

            if (page.IsRetry)
            {
                _pagesToCrawlRepo.Add(page);
            }
            else if(_crawledUrlRepo.AddIfNew(page.Uri))
            {
                _pagesToCrawlRepo.Add(page);
            }
        }

        public void Add(IEnumerable<PageToCrawl> pages)
        {
            if (pages == null) throw new ArgumentNullException("pages");

            foreach (var page in pages) Add(page);
        }


        public PageToCrawl GetNext()
        {
            return _pagesToCrawlRepo.GetNext();
        }

        public bool IsUriKnown(Uri uri)
        {
            var known = _crawledUrlRepo.Contains(uri);
            if (known) return known;

            return _scheduledPageRepo.IsVisitedUri(uri);
        }
        public void AddKnownUri(Uri uri)
        {
            _crawledUrlRepo.AddIfNew(uri);
        }

        public void Clear()
        {
            _pagesToCrawlRepo.Clear();
        }

        public void Dispose()
        {
            if (_pagesToCrawlRepo != null) _pagesToCrawlRepo.Dispose();
            if (_crawledUrlRepo != null) _crawledUrlRepo.Dispose();
        }
    }
}

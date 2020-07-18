using Abot2.Poco;

using Microsoft.Extensions.Logging;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using ZDeals.Engine.Data;

namespace ZDeals.Engine.Core.Components
{
    public class StoreScheduler : IStoreScheduler
    {
        private readonly EngineDbContext _dbContext;
        private readonly ILogger<StoreScheduler> _logger;

        private ConcurrentQueue<PageToCrawl> _pages;
        private ConcurrentDictionary<string, byte> _crawledPages;

        public StoreScheduler(EngineDbContext dbContext, ILogger<StoreScheduler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            _pages = new ConcurrentQueue<PageToCrawl>();
            _crawledPages = new ConcurrentDictionary<string, byte>();
        }

        public int Count => _pages.Count;

        public PageToCrawl GetNext()
        {
            var success = _pages.TryDequeue(out PageToCrawl page);
            return success ? page : null;
        }

        public void Add(PageToCrawl page)
        {
            var url = page.Uri.AbsoluteUri;
            if(_crawledPages.TryAdd(url, 0))
            {
                _pages.Enqueue(page);

                _logger.LogDebug($"Added into scheduler: {url}");
            }
        }

        public void Add(IEnumerable<PageToCrawl> pages)
        {
            foreach (var page in pages) Add(page);
        }

        public void AddKnownUri(Uri uri)
        {
            _crawledPages.TryAdd(uri.AbsoluteUri, 0);
        }

        public bool IsUriKnown(Uri uri)
        {
            return _crawledPages.ContainsKey(uri.AbsoluteUri);
        }

        public void Clear()
        {
            _pages.Clear();
        }

        public void Dispose()
        {
            _crawledPages = null;
            _pages = null;
        }


        public void AddTrackedPages(string store)
        {
            if (string.IsNullOrEmpty(store)) return;

            var tracked = _dbContext.TrackedPages.Where(x => x.Store == store);
            foreach(var t in tracked)
            {
                var page = new PageToCrawl
                {
                    Uri = new Uri(t.Url),
                    IsInternal = true,
                    IsRoot = false,
                    PageBag = new ExpandoObject()
                };
                Add(page);
            }
        }

    }
}
